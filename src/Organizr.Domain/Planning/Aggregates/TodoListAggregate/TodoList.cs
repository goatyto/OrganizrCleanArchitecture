﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.TodoListAggregate
{
    public class TodoList : Entity<Guid>, IAggregateRoot
    {
        public string CreatorUserId { get; private set; }
        public Guid? UserGroupId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }

        private readonly List<TodoItem> _items;
        public IReadOnlyCollection<TodoItem> Items => _items.AsReadOnly();

        private readonly List<TodoSubList> _subLists;
        public IReadOnlyCollection<TodoSubList> SubLists => _subLists.AsReadOnly();

        private TodoList()
        {
            _items = new List<TodoItem>();
            _subLists = new List<TodoSubList>();
        }

        public static TodoList Create(Guid id, string creatorUserId, string title, string description = null)
        {
            Guard.Against.Default(id, nameof(id));
            Guard.Against.NullOrWhiteSpace(creatorUserId, nameof(creatorUserId));
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            var todoList = new TodoList();

            todoList.Id = id;
            todoList.CreatorUserId = creatorUserId;
            todoList.Title = title;
            todoList.Description = description;

            todoList.AddDomainEvent(new TodoListCreated(todoList));

            return todoList;
        }

        internal static TodoList CreateShared(Guid id, string creatorUserId, Guid userGroupId, string title,
            string description = null)
        {
            Guard.Against.Default(userGroupId, nameof(userGroupId));

            var todoList = Create(id, creatorUserId, title, description);

            todoList.UserGroupId = userGroupId;

            todoList.AddDomainEvent(new TodoListCreated(todoList));

            return todoList;
        }

        public void Edit(string title, string description = null)
        {
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            Title = title;
            Description = description;

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void AddSubList(string title, string description = null)
        {
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            var subList = new TodoSubList(GetNextSubListId(), title, GetNextSubListOrdinal(), description);
            _subLists.Add(subList);

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void EditSubList(int subListId, string title, string description = null)
        {
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            var subList = FindSubList(subListId);

            subList.Edit(title, description);

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void MoveSubList(int subListId, int destinationOrdinal)
        {
            AssertSubListPositionValid(destinationOrdinal);

            var targetSubList = FindSubList(subListId);

            var initialOrdinal = targetSubList.Ordinal;

            if (initialOrdinal == destinationOrdinal)
                return;

            var isMovingUp = destinationOrdinal > initialOrdinal;
            var otherListsShiftDirection = isMovingUp ? ShiftDirection.Down : ShiftDirection.Up;
            var otherListsShiftStartOrdinal = isMovingUp ? initialOrdinal + 1 : destinationOrdinal;
            var otherListsShiftEndOrdinal = isMovingUp ? destinationOrdinal : initialOrdinal - 1;

            ShiftSubLists(otherListsShiftDirection, otherListsShiftStartOrdinal, otherListsShiftEndOrdinal);

            targetSubList.SetPosition(destinationOrdinal);

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void DeleteSubList(int subListId)
        {
            var subList = FindSubList(subListId);

            if (subList.IsDeleted)
                return;

            subList.Delete();

            ShiftSubLists(ShiftDirection.Down, subList.Ordinal + 1);

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void AddTodo(string title, string description = null, ClientDateUtc dueDateUtc = null, int? subListId = null)
        {
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            if (dueDateUtc != null)
            {
                AssertDueDateUtcValid(dueDateUtc);
            }

            var todo = new TodoItem(GetNextItemId(), title, GetNextItemPosition(subListId), description, dueDateUtc);

            if (subListId.HasValue)
            {
                var subList = FindSubList(subListId.Value);

                subList.AddTodo(todo);
            }
            else
            {
                _items.Add(todo);
            }

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void EditTodo(int todoId, string title, string description = null, ClientDateUtc dueDateUtc = null)
        {
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            var todo = FindTodoItem(todoId, out _);

            if (dueDateUtc != null)
            {
                AssertDueDateUtcValid(dueDateUtc);
            }

            todo.Edit(title, description, dueDateUtc);

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void SetCompletedTodo(int todoId, bool isCompleted = true)
        {
            var todo = FindTodoItem(todoId, out _);

            if (todo.IsCompleted == isCompleted)
                return;

            todo.SetCompleted(isCompleted);

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void MoveTodo(int todoId, TodoItemPosition destinationPosition, int? destinationSubListId = null)
        {
            var todoToBeMoved = FindTodoItem(todoId, out var sourceSubListId);

            AssertTodoItemPositionValid(destinationPosition, destinationSubListId);

            if (sourceSubListId == destinationSubListId && todoToBeMoved.Position == destinationPosition)
                return;

            TodoSubList sourceSubList = null, destinationSubList = null;

            if (sourceSubListId.HasValue)
            {
                sourceSubList = FindSubList(sourceSubListId.Value);
            }

            if (destinationSubListId.HasValue)
            {
                if (sourceSubListId == destinationSubListId)
                {
                    destinationSubList = sourceSubList;
                }
                else
                {
                    destinationSubList = FindSubList(destinationSubListId.Value);
                }
            }

            if (sourceSubListId == destinationSubListId)
            {
                var initialOrdinal = todoToBeMoved.Position;

                var isMovingUp = destinationPosition > initialOrdinal;
                var otherItemsShiftDirection = isMovingUp ? ShiftDirection.Down : ShiftDirection.Up;
                var otherItemsShiftStartOrdinal = isMovingUp ? initialOrdinal + 1 : destinationPosition;
                var otherItemsShiftEndOrdinal = isMovingUp ? destinationPosition : initialOrdinal - 1;

                var itemsToBeShifted = !sourceSubListId.HasValue ? Items : sourceSubList.Items;

                ShiftTodoItems(itemsToBeShifted, otherItemsShiftDirection, otherItemsShiftStartOrdinal, otherItemsShiftEndOrdinal);
            }
            else
            {
                var sourceItemsToBeShifted = !sourceSubListId.HasValue ? Items : sourceSubList.Items;
                ShiftTodoItems(sourceItemsToBeShifted, ShiftDirection.Down, todoToBeMoved.Position + 1);

                var destinationItemsToBeShifted = !destinationSubListId.HasValue ? Items : destinationSubList.Items;
                ShiftTodoItems(destinationItemsToBeShifted, ShiftDirection.Up, destinationPosition);

                if (!sourceSubListId.HasValue)
                    _items.Remove(todoToBeMoved);
                else
                    sourceSubList.RemoveTodo(todoToBeMoved);

                if (!destinationSubListId.HasValue)
                    _items.Add(todoToBeMoved);
                else
                    destinationSubList.AddTodo(todoToBeMoved);
            }

            todoToBeMoved.SetPosition(destinationPosition);

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void DeleteTodo(int todoId)
        {
            var todo = FindTodoItem(todoId, out var subListId);

            if (todo.IsDeleted)
                return;

            TodoSubList subList = null;

            if (subListId.HasValue)
            {
                subList = FindSubList(subListId.Value);
            }

            todo.Delete();

            var todoItemsToBeShifted = !subListId.HasValue ? Items : subList.Items;
            ShiftTodoItems(todoItemsToBeShifted, ShiftDirection.Down, todo.Position + 1);

            AddDomainEvent(new TodoListUpdated(this));
        }

        private int GetNextSubListId()
        {
            if (!SubLists.Any()) return 1;
            return SubLists.Max(sl => sl.Id) + 1;
        }

        private int GetNextSubListOrdinal()
        {
            return SubLists.Count(sl => !sl.IsDeleted) + 1;
        }

        private int GetNextItemId()
        {
            var allTodoItemsCount = Items.Union(SubLists.SelectMany(sl => sl.Items)).Count();
            return allTodoItemsCount + 1;
        }

        private TodoItemPosition GetNextItemPosition(int? subListId = null)
        {
            var items = !subListId.HasValue ? Items : FindSubList(subListId.Value).Items;

            return new TodoItemPosition(items.Count(item => !item.IsDeleted) + 1);
        }

        private enum ShiftDirection
        {
            Up = 1,
            Down = 2
        }

        private void ShiftSubLists(ShiftDirection direction, int startOrdinal, int? endOrdinal = null)
        {
            var subListsToBeShifted = SubLists.Where(sl =>
                    !sl.IsDeleted && sl.Ordinal >= startOrdinal && (!endOrdinal.HasValue || sl.Ordinal <= endOrdinal))
                .OrderBy(sl => sl.Ordinal)
                .ToList();

            for (int i = 0; i < subListsToBeShifted.Count; i++)
            {
                int newCalculatedOrdinal;

                switch (direction)
                {
                    case ShiftDirection.Down:
                        newCalculatedOrdinal = subListsToBeShifted[i].Ordinal - 1;
                        break;
                    case ShiftDirection.Up:
                    default:
                        newCalculatedOrdinal = subListsToBeShifted[i].Ordinal + 1;
                        break;
                }

                subListsToBeShifted[i].SetPosition(newCalculatedOrdinal);
            }
        }

        private void ShiftTodoItems(IEnumerable<TodoItem> items, ShiftDirection direction, TodoItemPosition startPosition, TodoItemPosition endPosition = null)
        {
            var todoItemsToBeShifted = items.Where(ti =>
                    !ti.IsDeleted && ti.Position >= startPosition && (endPosition == null || ti.Position <= endPosition))
                .OrderBy(sl => sl.Position)
                .ToList();

            for (int i = 0; i < todoItemsToBeShifted.Count; i++)
            {
                TodoItemPosition newCalculatedOrdinal;

                switch (direction)
                {
                    case ShiftDirection.Down:
                        newCalculatedOrdinal = i + startPosition - 1;
                        break;
                    case ShiftDirection.Up:
                    default:
                        newCalculatedOrdinal = i + startPosition + 1;
                        break;
                }

                todoItemsToBeShifted[i].SetPosition(newCalculatedOrdinal);
            }
        }

        private TodoSubList FindSubList(int subListId)
        {
            var subList = SubLists.SingleOrDefault(sl => !sl.IsDeleted && sl.Id == subListId);

            if (subList == null)
                throw new TodoListException($"Sublist with id \"{subListId}\" does not exist in list \"{Id}\".");

            return subList;
        }

        private TodoItem FindTodoItem(int todoId, out int? subListId)
        {
            subListId = null;

            var todo = Items.SingleOrDefault(item => !item.IsDeleted && item.Id == todoId);

            if (todo == null)
            {
                foreach (var subList in SubLists.Where(sl => !sl.IsDeleted))
                {
                    todo = subList.Items.SingleOrDefault(it => !it.IsDeleted && it.Id == todoId);

                    if (todo == null) continue;
                    
                    subListId = subList.Id;
                    break;
                }
            }

            if (todo == null)
                throw new TodoListException($"Todo item with id \"{todoId}\" does not exist in list \"{Id}\".");

            return todo;
        }

        public void AssertSubListPositionValid(int ordinal)
        {
            if (ordinal < 1 || ordinal > SubLists.Count)
                throw new TodoListException("Provided sublist ordinal is out of range.");
        }

        public void AssertTodoItemPositionValid(TodoItemPosition position, int? subListId = null)
        {
            var itemRangeToBeChecked =
                !subListId.HasValue ? Items : FindSubList(subListId.Value).Items;

            if (position < 1 || position > itemRangeToBeChecked.Count)
                throw new TodoListException("Provided todo item position is out of range.");
        }

        public void AssertDueDateUtcValid(ClientDateUtc clientDateUtc)
        {
            if (clientDateUtc.IsBeforeClientToday)
                throw new TodoListException($"Due date cannot be before client today.");
        }
    }
}
