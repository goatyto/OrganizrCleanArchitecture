using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ardalis.GuardClauses;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.TodoListAggregate
{
    public class TodoList : Entity<TodoListId>, IAggregateRoot
    {
        protected override TodoListId Identity => TodoListId;
        public TodoListId TodoListId { get; private set; }
        public CreatorUser CreatorUser { get; private set; }
        public UserGroupId UserGroupId { get; private set; }
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

        public static TodoList Create(TodoListId id, CreatorUser creatorUser, string title, string description = null)
        {
            Guard.Against.Default(id, nameof(id));
            Guard.Against.Null(creatorUser, nameof(creatorUser));
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            var todoList = new TodoList();

            todoList.TodoListId = id;
            todoList.CreatorUser = creatorUser;
            todoList.Title = title;
            todoList.Description = description;

            todoList.AddDomainEvent(new TodoListCreated(todoList));

            return todoList;
        }

        internal static TodoList CreateShared(TodoListId id, CreatorUser creatorUser, UserGroupId userGroupId, string title,
            string description = null)
        {
            Guard.Against.Default(userGroupId, nameof(userGroupId));

            var todoList = Create(id, creatorUser, title, description);

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

            var subList = new TodoSubList(GetNextSubListId(), title, GetNextSubListPosition(), description);
            _subLists.Add(subList);

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void EditSubList(TodoSubListId subListId, string title, string description = null)
        {
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            var subList = FindSubList(subListId);

            subList.Edit(title, description);

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void MoveSubList(TodoSubListId subListId, TodoSubListPosition destinationPosition)
        {
            AssertSubListPositionValid(destinationPosition);

            var targetSubList = FindSubList(subListId);

            var initialOrdinal = targetSubList.Position;

            if (initialOrdinal == destinationPosition)
                return;

            var isMovingUp = destinationPosition > initialOrdinal;
            var otherListsShiftDirection = isMovingUp ? ShiftDirection.Down : ShiftDirection.Up;
            var otherListsShiftStartOrdinal = isMovingUp ? initialOrdinal + 1 : destinationPosition;
            var otherListsShiftEndOrdinal = isMovingUp ? destinationPosition : initialOrdinal - 1;

            ShiftSubLists(otherListsShiftDirection, otherListsShiftStartOrdinal, otherListsShiftEndOrdinal);

            targetSubList.SetPosition(destinationPosition);

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void DeleteSubList(TodoSubListId subListId)
        {
            var subList = FindSubList(subListId);

            if (subList.IsDeleted)
                return;

            subList.Delete();

            ShiftSubLists(ShiftDirection.Down, subList.Position + 1);

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void AddTodo(string title, string description = null, ClientDateUtc dueDateUtc = null, TodoSubListId subListId = null)
        {
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            if (dueDateUtc != null)
            {
                AssertDueDateUtcValid(dueDateUtc);
            }

            var todo = new TodoItem(GetNextItemId(), title, GetNextItemPosition(subListId), description, dueDateUtc);

            if (subListId != null)
            {
                var subList = FindSubList(subListId);

                subList.AddTodo(todo);
            }
            else
            {
                _items.Add(todo);
            }

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void EditTodo(TodoItemId todoId, string title, string description = null, ClientDateUtc dueDateUtc = null)
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

        public void SetCompletedTodo(TodoItemId todoId, bool isCompleted = true)
        {
            var todo = FindTodoItem(todoId, out _);

            if (todo.IsCompleted == isCompleted)
                return;

            todo.SetCompleted(isCompleted);

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void MoveTodo(TodoItemId todoId, TodoItemPosition destinationPosition, TodoSubListId destinationSubListId = null)
        {
            var todoToBeMoved = FindTodoItem(todoId, out var sourceSubListId);

            AssertTodoItemPositionValid(destinationPosition, destinationSubListId);

            if (sourceSubListId == destinationSubListId && todoToBeMoved.Position == destinationPosition)
                return;

            TodoSubList sourceSubList = null, destinationSubList = null;

            if (sourceSubListId != null)
            {
                sourceSubList = FindSubList(sourceSubListId);
            }

            if (destinationSubListId != null)
            {
                if (sourceSubListId == destinationSubListId)
                {
                    destinationSubList = sourceSubList;
                }
                else
                {
                    destinationSubList = FindSubList(destinationSubListId);
                }
            }

            if (sourceSubListId == destinationSubListId)
            {
                var initialOrdinal = todoToBeMoved.Position;

                var isMovingUp = destinationPosition > initialOrdinal;
                var otherItemsShiftDirection = isMovingUp ? ShiftDirection.Down : ShiftDirection.Up;
                var otherItemsShiftStartOrdinal = isMovingUp ? initialOrdinal + 1 : destinationPosition;
                var otherItemsShiftEndOrdinal = isMovingUp ? destinationPosition : initialOrdinal - 1;

                var itemsToBeShifted = sourceSubList == null ? Items : sourceSubList.Items;

                ShiftTodoItems(itemsToBeShifted, otherItemsShiftDirection, otherItemsShiftStartOrdinal, otherItemsShiftEndOrdinal);
            }
            else
            {
                var sourceItemsToBeShifted = sourceSubListId == null ? Items : sourceSubList.Items;
                ShiftTodoItems(sourceItemsToBeShifted, ShiftDirection.Down, todoToBeMoved.Position + 1);

                var destinationItemsToBeShifted = destinationSubListId == null ? Items : destinationSubList.Items;
                ShiftTodoItems(destinationItemsToBeShifted, ShiftDirection.Up, destinationPosition);

                if (sourceSubListId == null)
                    _items.Remove(todoToBeMoved);
                else
                    sourceSubList.RemoveTodo(todoToBeMoved);

                if (destinationSubListId == null)
                    _items.Add(todoToBeMoved);
                else
                    destinationSubList.AddTodo(todoToBeMoved);
            }

            todoToBeMoved.SetPosition(destinationPosition);

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void DeleteTodo(TodoItemId todoId)
        {
            var todo = FindTodoItem(todoId, out var subListId);

            if (todo.IsDeleted)
                return;

            TodoSubList subList = null;

            if (subListId != null)
            {
                subList = FindSubList(subListId);
            }

            todo.Delete();

            var todoItemsToBeShifted = subListId == null ? Items : subList.Items;
            ShiftTodoItems(todoItemsToBeShifted, ShiftDirection.Down, todo.Position + 1);

            AddDomainEvent(new TodoListUpdated(this));
        }

        private TodoSubListId GetNextSubListId()
        {
            if (!SubLists.Any()) return TodoSubListId.Create(1);
            return TodoSubListId.Create(SubLists.Max(sl => sl.TodoSubListId).Id + 1);
        }

        private TodoSubListPosition GetNextSubListPosition()
        {
            return new TodoSubListPosition(SubLists.Count(sl => !sl.IsDeleted) + 1);
        }

        private TodoItemId GetNextItemId()
        {
            var allTodoItemsCount = Items.Union(SubLists.SelectMany(sl => sl.Items)).Count();
            return new TodoItemId(allTodoItemsCount + 1);
        }

        private TodoItemPosition GetNextItemPosition(TodoSubListId subListId = null)
        {
            var items = subListId == null ? Items : FindSubList(subListId).Items;

            return new TodoItemPosition(items.Count(item => !item.IsDeleted) + 1);
        }

        private enum ShiftDirection
        {
            Up = 1,
            Down = 2
        }

        private void ShiftSubLists(ShiftDirection direction, TodoSubListPosition startPosition, TodoSubListPosition endPosition = null)
        {
            var subListsToBeShifted = SubLists.Where(sl =>
                    !sl.IsDeleted && sl.Position >= startPosition && (endPosition == null || sl.Position <= endPosition))
                .OrderBy(sl => sl.Position)
                .ToList();

            for (int i = 0; i < subListsToBeShifted.Count; i++)
            {
                TodoSubListPosition newCalculatedPosition;

                switch (direction)
                {
                    case ShiftDirection.Down:
                        newCalculatedPosition = i + startPosition - 1;
                        break;
                    case ShiftDirection.Up:
                    default:
                        newCalculatedPosition = i + startPosition + 1;
                        break;
                }

                subListsToBeShifted[i].SetPosition(newCalculatedPosition);
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
                TodoItemPosition newCalculatedPosition;

                switch (direction)
                {
                    case ShiftDirection.Down:
                        newCalculatedPosition = i + startPosition - 1;
                        break;
                    case ShiftDirection.Up:
                    default:
                        newCalculatedPosition = i + startPosition + 1;
                        break;
                }

                todoItemsToBeShifted[i].SetPosition(newCalculatedPosition);
            }
        }

        private TodoSubList FindSubList(TodoSubListId subListId)
        {
            var subList = SubLists.SingleOrDefault(sl => !sl.IsDeleted && sl.TodoSubListId == subListId);

            if (subList == null)
                throw new TodoListException($"Sublist with id \"{subListId}\" does not exist in list \"{Identity}\".");

            return subList;
        }

        private TodoItem FindTodoItem(TodoItemId todoId, out TodoSubListId subListId)
        {
            subListId = null;

            var todo = Items.SingleOrDefault(item => !item.IsDeleted && item.TodoItemId == todoId);

            if (todo == null)
            {
                foreach (var subList in SubLists.Where(sl => !sl.IsDeleted))
                {
                    todo = subList.Items.SingleOrDefault(it => !it.IsDeleted && it.TodoItemId == todoId);

                    if (todo == null) continue;

                    subListId = subList.TodoSubListId;
                    break;
                }
            }

            if (todo == null)
                throw new TodoListException($"Todo item with id \"{todoId}\" does not exist in list \"{Identity}\".");

            return todo;
        }

        public void AssertSubListPositionValid(TodoSubListPosition position)
        {
            if (position < 1 || position > SubLists.Count)
                throw new TodoListException("Provided sublist ordinal is out of range.");
        }

        public void AssertTodoItemPositionValid(TodoItemPosition position, TodoSubListId subListId = null)
        {
            var itemRangeToBeChecked =
                subListId == null ? Items : FindSubList(subListId).Items;

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
