using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using Organizr.Domain.Guards;
using Organizr.Domain.Planning.Services;
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
            Guard.Against.NullQueryResult(subList, nameof(subListId));

            if (subList.IsDeleted)
                throw new TodoSubListDeletedException(Id, subListId);

            subList.Edit(title, description);

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void MoveSubList(int subListId, int destinationOrdinal)
        {
            Guard.Against.OutOfRange(destinationOrdinal, nameof(destinationOrdinal), 1, SubLists.Count(sl => !sl.IsDeleted));

            var targetSubList = FindSubList(subListId);
            Guard.Against.NullQueryResult(targetSubList, nameof(subListId));

            if (targetSubList.IsDeleted)
                throw new TodoSubListDeletedException(Id, subListId);

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
            Guard.Against.NullQueryResult(subList, nameof(subListId));

            if (subList.IsDeleted)
                return;

            subList.Delete();

            ShiftSubLists(ShiftDirection.Down, subList.Ordinal + 1);

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void AddTodo(string title, string description = null, DateTime? dueDateUtc = null,
            int? clientTimeZoneOffsetInMinutes = null, ClientDateValidator clientDateValidator = null,
            int? subListId = null)
        {
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            if (dueDateUtc.HasValue)
            {
                Guard.Against.Null(clientTimeZoneOffsetInMinutes, nameof(clientTimeZoneOffsetInMinutes));
                Guard.Against.Null(clientDateValidator, nameof(clientDateValidator));

                Guard.Against.HavingTimeComponent(dueDateUtc.Value.AddMinutes(-clientTimeZoneOffsetInMinutes.Value),
                    nameof(dueDateUtc));

                if (clientDateValidator.IsDateBeforeClientToday(dueDateUtc.Value, clientTimeZoneOffsetInMinutes.Value))
                    throw new DueDateBeforeTodayException(Id, dueDateUtc.Value);
            }

            var todo = new TodoItem(GetNextItemId(), title, GetNextItemOrdinal(subListId), description, dueDateUtc);

            if (subListId.HasValue)
            {
                var subList = FindSubList(subListId.Value);
                Guard.Against.NullQueryResult(subList, nameof(subListId));

                if (subList.IsDeleted)
                    throw new TodoSubListDeletedException(Id, subListId.Value);

                subList.AddTodo(todo);
            }
            else
            {
                _items.Add(todo);
            }

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void EditTodo(int todoId, string title, string description = null, DateTime? dueDateUtc = null,
            int? clientTimeZoneOffsetInMinutes = null, ClientDateValidator clientDateValidator = null)
        {
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            if (dueDateUtc.HasValue)
            {
                Guard.Against.Null(clientTimeZoneOffsetInMinutes, nameof(clientTimeZoneOffsetInMinutes));
                Guard.Against.Null(clientDateValidator, nameof(clientDateValidator));

                Guard.Against.HavingTimeComponent(dueDateUtc.Value.AddMinutes(-clientTimeZoneOffsetInMinutes.Value),
                    nameof(dueDateUtc));

                if (clientDateValidator.IsDateBeforeClientToday(dueDateUtc.Value, clientTimeZoneOffsetInMinutes.Value))
                    throw new DueDateBeforeTodayException(Id, dueDateUtc.Value);
            }

            var todo = FindTodoItem(todoId, out var subListId);
            Guard.Against.NullQueryResult(todo, nameof(todoId));

            if (subListId.HasValue)
            {
                var subList = FindSubList(subListId.Value);
                Guard.Against.NullQueryResult(subList, nameof(subListId));

                if (subList.IsDeleted)
                    throw new TodoSubListDeletedException(Id, subListId.Value);
            }

            if (todo.IsCompleted)
                throw new TodoItemCompletedException(Id, todoId);

            if (todo.IsDeleted)
                throw new TodoItemDeletedException(Id, todoId);

            todo.Edit(title, description, dueDateUtc);

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void SetCompletedTodo(int todoId, bool isCompleted = true)
        {
            var todo = FindTodoItem(todoId, out var subListId);
            Guard.Against.NullQueryResult(todo, nameof(todoId));

            if (subListId.HasValue)
            {
                var subList = FindSubList(subListId.Value);
                Guard.Against.NullQueryResult(subList, nameof(subListId));

                if (subList.IsDeleted)
                    throw new TodoSubListDeletedException(Id, subListId.Value);
            }

            if (todo.IsDeleted)
                throw new TodoItemDeletedException(Id, todoId);

            if (todo.IsCompleted == isCompleted)
                return;

            todo.SetCompleted(isCompleted);

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void MoveTodo(int todoId, int destinationOrdinal, int? destinationSubListId = null)
        {
            if (destinationSubListId.HasValue)
            {
                var destinationSubList = FindSubList(destinationSubListId.Value);
                Guard.Against.NullQueryResult(destinationSubList, nameof(destinationSubListId));

                if (destinationSubList.IsDeleted)
                    throw new TodoSubListDeletedException(Id, destinationSubListId.Value);

                Guard.Against.OutOfRange(destinationOrdinal, nameof(destinationOrdinal), 1,
                    destinationSubList.Items.Count(item => !item.IsDeleted));
            }
            else
            {
                Guard.Against.OutOfRange(destinationOrdinal, nameof(destinationOrdinal), 1,
                    Items.Count(item => !item.IsDeleted));
            }

            var todoToBeMoved = FindTodoItem(todoId, out var sourceSubListId);
            Guard.Against.NullQueryResult(todoToBeMoved, nameof(todoId));

            if (sourceSubListId.HasValue)
            {
                var sourceSubList = FindSubList(sourceSubListId.Value);
                Guard.Against.NullQueryResult(sourceSubList, nameof(todoId));

                if (sourceSubList.IsDeleted)
                    throw new TodoSubListDeletedException(Id, sourceSubListId.Value);
            }

            if (todoToBeMoved.IsDeleted)
                throw new TodoItemDeletedException(Id, todoId);

            if (sourceSubListId == destinationSubListId && todoToBeMoved.Ordinal == destinationOrdinal)
                return;

            if (sourceSubListId == destinationSubListId)
            {
                var initialOrdinal = todoToBeMoved.Ordinal;

                var isMovingUp = destinationOrdinal > initialOrdinal;
                var otherItemsShiftDirection = isMovingUp ? ShiftDirection.Down : ShiftDirection.Up;
                var otherItemsShiftStartOrdinal = isMovingUp ? initialOrdinal + 1 : destinationOrdinal;
                var otherItemsShiftEndOrdinal = isMovingUp ? destinationOrdinal : initialOrdinal - 1;

                var itemsToBeShifted = !sourceSubListId.HasValue ? Items : FindSubList(sourceSubListId.Value).Items;

                ShiftTodoItems(itemsToBeShifted, otherItemsShiftDirection, otherItemsShiftStartOrdinal, otherItemsShiftEndOrdinal);
            }
            else
            {
                var sourceItemsToBeShifted = !sourceSubListId.HasValue ? Items : FindSubList(sourceSubListId.Value).Items;
                ShiftTodoItems(sourceItemsToBeShifted, ShiftDirection.Down, todoToBeMoved.Ordinal + 1);

                var destinationItemsToBeShifted = !destinationSubListId.HasValue ? Items : FindSubList(destinationSubListId.Value).Items;
                ShiftTodoItems(destinationItemsToBeShifted, ShiftDirection.Up, destinationOrdinal);

                if (!sourceSubListId.HasValue)
                    _items.Remove(todoToBeMoved);
                else
                    FindSubList(sourceSubListId.Value).RemoveTodo(todoToBeMoved);

                if (!destinationSubListId.HasValue)
                    _items.Add(todoToBeMoved);
                else
                    FindSubList(destinationSubListId.Value).AddTodo(todoToBeMoved);
            }

            todoToBeMoved.SetPosition(destinationOrdinal);

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void DeleteTodo(int todoId)
        {
            var todo = FindTodoItem(todoId, out var subListId);
            Guard.Against.NullQueryResult(todo, nameof(todoId));

            if (subListId.HasValue)
            {
                var subList = FindSubList(subListId.Value);
                Guard.Against.NullQueryResult(subList, nameof(subListId));

                if (subList.IsDeleted)
                    throw new TodoSubListDeletedException(Id, subListId.Value);
            }

            if (todo.IsDeleted)
                return;

            todo.Delete();

            var todoItemsToBeShifted = !subListId.HasValue ? Items : FindSubList(subListId.Value).Items;
            ShiftTodoItems(todoItemsToBeShifted, ShiftDirection.Down, todo.Ordinal + 1);

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

        private int GetNextItemOrdinal(int? subListId = null)
        {
            var items = !subListId.HasValue ? Items : SubLists.SingleOrDefault(sl => sl.Id == subListId.Value)?.Items;
            Guard.Against.NullQueryResult(items, nameof(subListId));

            return items.Count(item => !item.IsDeleted) + 1;
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

        private void ShiftTodoItems(IEnumerable<TodoItem> items, ShiftDirection direction, int startOrdinal, int? endOrdinal = null)
        {
            var todoItemsToBeShifted = items.Where(ti =>
                    !ti.IsDeleted && ti.Ordinal >= startOrdinal && (!endOrdinal.HasValue || ti.Ordinal <= endOrdinal))
                .OrderBy(sl => sl.Ordinal)
                .ToList();

            for (int i = 0; i < todoItemsToBeShifted.Count; i++)
            {
                int newCalculatedOrdinal;

                switch (direction)
                {
                    case ShiftDirection.Down:
                        newCalculatedOrdinal = todoItemsToBeShifted[i].Ordinal - 1;
                        break;
                    case ShiftDirection.Up:
                    default:
                        newCalculatedOrdinal = todoItemsToBeShifted[i].Ordinal + 1;
                        break;
                }

                todoItemsToBeShifted[i].SetPosition(newCalculatedOrdinal);
            }
        }

        private TodoSubList FindSubList(int subListId)
        {
            return SubLists.SingleOrDefault(sl => sl.Id == subListId);
        }

        private TodoItem FindTodoItem(int todoId, out int? subListId)
        {
            subListId = null;

            var todo = Items.SingleOrDefault(item => item.Id == todoId);

            if (todo == null)
            {
                foreach (var subList in SubLists)
                {
                    todo = subList.Items.SingleOrDefault(it => it.Id == todoId);

                    if (todo != null)
                    {
                        subListId = subList.Id;
                        break;
                    }
                }
            }

            return todo;
        }
    }
}
