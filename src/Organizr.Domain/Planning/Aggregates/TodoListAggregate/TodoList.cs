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

            var subList = SubLists.SingleOrDefault(sl => sl.Id == subListId);

            Guard.Against.NullQueryResult(subList, nameof(subListId));

            if (subList.IsDeleted)
                throw new TodoSubListDeletedException(Id, subListId);

            subList.Edit(title, description);

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void MoveSubList(int subListId, int newOrdinal)
        {
            Guard.Against.OutOfRange(newOrdinal, nameof(newOrdinal), 1, SubLists.Count(sl => !sl.IsDeleted));

            var targetSubList = SubLists.SingleOrDefault(sl => sl.Id == subListId);

            Guard.Against.NullQueryResult(targetSubList, nameof(subListId));

            if (targetSubList.IsDeleted)
                throw new TodoSubListDeletedException(Id, subListId);

            var initialOrdinal = targetSubList.Ordinal;

            if (initialOrdinal == newOrdinal)
                return;

            var isMovingUp = newOrdinal > initialOrdinal;
            var otherListsShiftDirection = isMovingUp ? ShiftDirection.Down : ShiftDirection.Up;
            var otherListsShiftStartOrdinal = isMovingUp ? initialOrdinal + 1 : newOrdinal;
            var otherListsShiftEndOrdinal = isMovingUp ? newOrdinal : initialOrdinal - 1;

            ShiftSubLists(otherListsShiftDirection, otherListsShiftStartOrdinal, otherListsShiftEndOrdinal);

            targetSubList.SetPosition(newOrdinal);

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void DeleteSubList(int subListId)
        {
            var subList = SubLists.SingleOrDefault(sl => sl.Id == subListId);

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

            if (subListId.HasValue)
            {
                var subList = SubLists.SingleOrDefault(sl => sl.Id == subListId.Value);

                Guard.Against.NullQueryResult(subList, nameof(subListId));

                if (subList.IsDeleted)
                    throw new TodoSubListDeletedException(Id, subListId.Value);
            }

            var todo = new TodoItem(GetNextItemId(), title,
                GetNextItemOrdinal(subListId), description, dueDateUtc, subListId);
            _items.Add(todo);

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void EditTodo(int todoId, string title, string description = null, DateTime? dueDateUtc = null,
            int? clientTimeZoneOffsetInMinutes = null, ClientDateValidator clientDateValidator = null)
        {
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            var todo = Items.SingleOrDefault(item => item.Id == todoId);
            Guard.Against.NullQueryResult(todo, nameof(todoId));

            if (dueDateUtc.HasValue)
            {
                Guard.Against.Null(clientTimeZoneOffsetInMinutes, nameof(clientTimeZoneOffsetInMinutes));
                Guard.Against.Null(clientDateValidator, nameof(clientDateValidator));

                Guard.Against.HavingTimeComponent(dueDateUtc.Value.AddMinutes(-clientTimeZoneOffsetInMinutes.Value),
                    nameof(dueDateUtc));

                if (clientDateValidator.IsDateBeforeClientToday(dueDateUtc.Value, clientTimeZoneOffsetInMinutes.Value))
                    throw new DueDateBeforeTodayException(Id, dueDateUtc.Value);
            }

            var subListId = todo.SubListId;

            if (subListId.HasValue)
            {
                if (SubLists.Single(sl => sl.Id == subListId.Value).IsDeleted)
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
            var todo = Items.SingleOrDefault(item => item.Id == todoId);
            Guard.Against.NullQueryResult(todo, nameof(todoId));

            var subListId = todo.SubListId;

            if (subListId.HasValue)
            {
                if (SubLists.Single(sl => sl.Id == subListId.Value).IsDeleted)
                    throw new TodoSubListDeletedException(Id, subListId.Value);
            }

            if (todo.IsDeleted)
                throw new TodoItemDeletedException(Id, todoId);

            if (todo.IsCompleted == isCompleted)
                return;

            todo.SetCompleted(isCompleted);

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void MoveTodo(int todoId, int newOrdinal, int? newSubListId = null)
        {
            if (newSubListId.HasValue)
            {
                var subList = SubLists.SingleOrDefault(sl => sl.Id == newSubListId.Value);

                Guard.Against.NullQueryResult(subList, nameof(newSubListId));

                if (subList.IsDeleted)
                    throw new TodoSubListDeletedException(Id, newSubListId.Value);
            }

            Guard.Against.OutOfRange(newOrdinal, nameof(newOrdinal), 1,
                Items.Count(item => !item.IsDeleted && item.SubListId == newSubListId));

            var todoToBeMoved = Items.SingleOrDefault(item => item.Id == todoId);
            Guard.Against.NullQueryResult(todoToBeMoved, nameof(todoId));

            if (todoToBeMoved.SubListId.HasValue)
            {
                var sourceSubList = SubLists.Single(sl => sl.Id == todoToBeMoved.SubListId.Value);

                if (sourceSubList.IsDeleted)
                    throw new TodoSubListDeletedException(Id, todoToBeMoved.SubListId.Value);
            }

            if (todoToBeMoved.IsDeleted)
                throw new TodoItemDeletedException(Id, todoId);

            if (todoToBeMoved.SubListId == newSubListId && todoToBeMoved.Ordinal == newOrdinal)
                return;

            var sourceSubListId = todoToBeMoved.SubListId;
            var destinationSubListId = newSubListId;

            if (sourceSubListId == destinationSubListId)
            {
                var initialOrdinal = todoToBeMoved.Ordinal;

                var isMovingUp = newOrdinal > initialOrdinal;
                var otherItemsShiftDirection = isMovingUp ? ShiftDirection.Down : ShiftDirection.Up;
                var otherItemsShiftStartOrdinal = isMovingUp ? initialOrdinal + 1 : newOrdinal;
                var otherItemsShiftEndOrdinal = isMovingUp ? newOrdinal : initialOrdinal - 1;

                ShiftTodoItems(otherItemsShiftDirection, otherItemsShiftStartOrdinal, otherItemsShiftEndOrdinal, destinationSubListId);
            }
            else
            {
                ShiftTodoItems(ShiftDirection.Down, todoToBeMoved.Ordinal + 1, subListId: sourceSubListId);
                ShiftTodoItems(ShiftDirection.Up, newOrdinal, subListId: destinationSubListId);
            }

            todoToBeMoved.SetPosition(newOrdinal, newSubListId);

            AddDomainEvent(new TodoListUpdated(this));
        }

        public void DeleteTodo(int todoId)
        {
            var todo = Items.SingleOrDefault(item => item.Id == todoId);
            Guard.Against.NullQueryResult(todo, nameof(todoId));

            var subListId = todo.SubListId;

            if (subListId.HasValue)
            {
                if (SubLists.Single(sl => sl.Id == subListId.Value).IsDeleted)
                    throw new TodoSubListDeletedException(Id, subListId.Value);
            }

            if (todo.IsDeleted)
                return;

            todo.Delete();

            ShiftTodoItems(ShiftDirection.Down, todo.Ordinal + 1, subListId: todo.SubListId);

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
            if (!Items.Any()) return 1;
            return Items.Max(i => i.Id) + 1;
        }

        private int GetNextItemOrdinal(int? subListId = null)
        {
            return Items.Count(item => item.SubListId == subListId && !item.IsDeleted) + 1;
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

        private void ShiftTodoItems(ShiftDirection direction, int startOrdinal, int? endOrdinal = null,
            int? subListId = null)
        {
            var todoItemsToBeShifted = Items.Where(sl =>
                    sl.SubListId == subListId && !sl.IsDeleted && sl.Ordinal >= startOrdinal &&
                    (!endOrdinal.HasValue || sl.Ordinal <= endOrdinal))
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

                todoItemsToBeShifted[i].SetPosition(newCalculatedOrdinal, todoItemsToBeShifted[i].SubListId);
            }
        }
    }
}
