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
        public string CreatorUserId { get; }
        public string Title { get; private set; }
        public string Description { get; private set; }

        private readonly List<TodoItem> _items;
        public IReadOnlyCollection<TodoItem> Items => _items.AsReadOnly();

        private readonly List<TodoSubList> _subLists;
        public IReadOnlyCollection<TodoSubList> SubLists => _subLists.AsReadOnly();

        private TodoList()
        {

        }

        public TodoList(Guid id, string creatorUserId, string title, string description = null) : this()
        {
            Guard.Against.Default(id, nameof(id));
            Guard.Against.NullOrWhiteSpace(creatorUserId, nameof(creatorUserId));
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            Id = id;
            CreatorUserId = creatorUserId;
            Title = title;
            Description = description;

            _items = new List<TodoItem>();
            _subLists = new List<TodoSubList>();
        }

        public void Edit(string title, string description = null)
        {
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            Title = title;
            Description = description;
        }

        public void AddSubList(string title, string description = null)
        {
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            var subList = new TodoSubList(GetNextSubListId(), Id, title, GetNextSubListOrdinal(), description);
            _subLists.Add(subList);
        }

        public void EditSubList(int subListId, string title, string description = null)
        {
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            var subList = SubLists.SingleOrDefault(sl => sl.Id == subListId);

            Guard.Against.NullQueryResult(subList, nameof(subListId));

            if (subList.IsDeleted)
                throw new TodoSubListDeletedException(Id, subListId);

            subList.Edit(title, description);
        }

        public void MoveSubList(int subListId, int newOrdinal)
        {
            Guard.Against.OutOfRange(newOrdinal, nameof(newOrdinal), 1, SubLists.Count);

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

            targetSubList.SetOrdinal(newOrdinal);
        }

        public void DeleteSubList(int subListId)
        {
            var subList = SubLists.SingleOrDefault(sl => sl.Id == subListId);

            Guard.Against.NullQueryResult(subList, nameof(subListId));

            if (subList.IsDeleted)
                return;

            subList.Delete();

            ShiftSubLists(ShiftDirection.Down, subList.Ordinal + 1);
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

            var todo = new TodoItem(GetNextItemId(), Id, title,
                new TodoItemPosition(GetNextItemOrdinal(subListId), subListId), description, dueDateUtc);
            _items.Add(todo);
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

            var subListId = todo.Position.SubListId;

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
        }

        public void SetCompletedTodo(int todoId, bool isCompleted = true)
        {
            var todo = Items.SingleOrDefault(item => item.Id == todoId);
            Guard.Against.NullQueryResult(todo, nameof(todoId));

            var subListId = todo.Position.SubListId;

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
        }

        public void MoveTodo(int todoId, TodoItemPosition newPosition)
        {
            if (newPosition.SubListId.HasValue)
            {
                var subList = SubLists.SingleOrDefault(sl => sl.Id == newPosition.SubListId.Value);

                Guard.Against.NullQueryResult(subList, nameof(newPosition.SubListId));

                if (subList.IsDeleted)
                    throw new TodoSubListDeletedException(Id, newPosition.SubListId.Value);
            }

            Guard.Against.OutOfRange(newPosition.Ordinal, nameof(newPosition), 1,
                Items.Count(item => !item.IsDeleted && item.Position.SubListId == newPosition.SubListId));

            var todoToBeMoved = Items.SingleOrDefault(item => item.Id == todoId);
            Guard.Against.NullQueryResult(todoToBeMoved, nameof(todoId));

            if (todoToBeMoved.Position.SubListId.HasValue)
            {
                var sourceSubList = SubLists.Single(sl => sl.Id == todoToBeMoved.Position.SubListId.Value);

                if (sourceSubList.IsDeleted)
                    throw new TodoSubListDeletedException(Id, todoToBeMoved.Position.SubListId.Value);
            }

            if (todoToBeMoved.IsDeleted)
                throw new TodoItemDeletedException(Id, todoId);

            if (todoToBeMoved.Position == newPosition)
                return;

            var sourceSubListId = todoToBeMoved.Position.SubListId;
            var destinationSubListId = newPosition.SubListId;

            if (sourceSubListId == destinationSubListId)
            {
                var initialOrdinal = todoToBeMoved.Position.Ordinal;
                var newOrdinal = newPosition.Ordinal;

                var isMovingUp = newOrdinal > initialOrdinal;
                var otherItemsShiftDirection = isMovingUp ? ShiftDirection.Down : ShiftDirection.Up;
                var otherItemsShiftStartOrdinal = isMovingUp ? initialOrdinal + 1 : newOrdinal;
                var otherItemsShiftEndOrdinal = isMovingUp ? newOrdinal : initialOrdinal - 1;

                ShiftTodoItems(otherItemsShiftDirection, otherItemsShiftStartOrdinal, otherItemsShiftEndOrdinal, destinationSubListId);
            }
            else
            {
                ShiftTodoItems(ShiftDirection.Down, todoToBeMoved.Position.Ordinal + 1, subListId: sourceSubListId);
                ShiftTodoItems(ShiftDirection.Up, newPosition.Ordinal, subListId: destinationSubListId);
            }

            //if (sourceSubListId == destinationSubListId)
            //{
            //    var listTransformationStartOrdinal = Math.Min(todoToBeMoved.Position.Ordinal, newPosition.Ordinal);
            //    var listTransformationEndOrdinal = Math.Max(todoToBeMoved.Position.Ordinal, newPosition.Ordinal);

            //    var isMovingUp = todoToBeMoved.Position.Ordinal < newPosition.Ordinal;

            //    var otherItems = Items.Where(item =>
            //        item.Position.SubListId == destinationSubListId && !item.IsDeleted && item.Id != todoId &&
            //        item.Position.Ordinal >= listTransformationStartOrdinal &&
            //        item.Position.Ordinal <= listTransformationEndOrdinal).ToList();

            //    for (int i = 0; i < otherItems.Count; i++)
            //    {
            //        var newCalculatedOrdinal = isMovingUp
            //            ? i + listTransformationStartOrdinal
            //            : i + listTransformationStartOrdinal + 1;

            //        otherItems[i].SetPosition(new TodoItemPosition(newCalculatedOrdinal, destinationSubListId));
            //    }
            //}
            //else
            //{
            //    var sourceListOtherItems = Items.Where(item =>
            //        item.Position.SubListId == sourceSubListId && !item.IsDeleted && item.Id != todoId &&
            //        item.Position.Ordinal > todoToBeMoved.Position.Ordinal).ToList();

            //    for (int i = 0; i < sourceListOtherItems.Count; i++)
            //    {
            //        sourceListOtherItems[i].SetPosition(new TodoItemPosition(i + todoToBeMoved.Position.Ordinal, sourceSubListId));
            //    }

            //    var destinationListOtherMovedItems = Items.Where(item =>
            //        item.Position.SubListId == destinationSubListId && !item.IsDeleted &&
            //        item.Position.Ordinal >= newPosition.Ordinal).ToList();

            //    for (int i = 0; i < destinationListOtherMovedItems.Count; i++)
            //    {
            //        destinationListOtherMovedItems[i].SetPosition(new TodoItemPosition(i + newPosition.Ordinal + 1, destinationSubListId));
            //    }
            //}

            todoToBeMoved.SetPosition(newPosition);
        }

        public void DeleteTodo(int todoId)
        {
            var todo = Items.SingleOrDefault(item => item.Id == todoId);
            Guard.Against.NullQueryResult(todo, nameof(todoId));

            var subListId = todo.Position.SubListId;

            if (subListId.HasValue)
            {
                if (SubLists.Single(sl => sl.Id == subListId.Value).IsDeleted)
                    throw new TodoSubListDeletedException(Id, subListId.Value);
            }

            if (todo.IsDeleted)
                return;

            todo.Delete();

            ShiftTodoItems(ShiftDirection.Down, todo.Position.Ordinal + 1, subListId: todo.Position.SubListId);
        }

        private int GetNextSubListId()
        {
            return SubLists.Count + 1;
        }

        private int GetNextSubListOrdinal()
        {
            return SubLists.Count(sl => !sl.IsDeleted) + 1;
        }

        private int GetNextItemId()
        {
            return Items.Count + 1;
        }

        private int GetNextItemOrdinal(int? subListId = null)
        {
            return Items.Count(item => item.Position.SubListId == subListId && !item.IsDeleted) + 1;
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

                subListsToBeShifted[i].SetOrdinal(newCalculatedOrdinal);
            }
        }

        private void ShiftTodoItems(ShiftDirection direction, int startOrdinal, int? endOrdinal = null,
            int? subListId = null)
        {
            var todoItemsToBeShifted = Items.Where(sl =>
                    sl.Position.SubListId == subListId && !sl.IsDeleted && sl.Position.Ordinal >= startOrdinal &&
                    (!endOrdinal.HasValue || sl.Position.Ordinal <= endOrdinal))
                .OrderBy(sl => sl.Position.Ordinal)
                .ToList();

            for (int i = 0; i < todoItemsToBeShifted.Count; i++)
            {
                int newCalculatedOrdinal;

                switch (direction)
                {
                    case ShiftDirection.Down:
                        newCalculatedOrdinal = todoItemsToBeShifted[i].Position.Ordinal - 1;
                        break;
                    case ShiftDirection.Up:
                    default:
                        newCalculatedOrdinal = todoItemsToBeShifted[i].Position.Ordinal + 1;
                        break;
                }

                todoItemsToBeShifted[i].SetPosition(new TodoItemPosition(newCalculatedOrdinal, subListId));
            }
        }
    }
}
