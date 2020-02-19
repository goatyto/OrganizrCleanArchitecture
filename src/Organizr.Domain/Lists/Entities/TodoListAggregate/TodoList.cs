using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Lists.Entities.TodoListAggregate
{
    public class TodoList : ResourceEntity, IAggregateRoot
    {
        public string Title { get; protected set; }

        public string Description { get; protected set; }

        public DateTime? DueDate => Items.Any(item => item.DueDate.HasValue)
            ? Items.Where(item => item.DueDate.HasValue).Max(item => item.DueDate)
            : null;

        protected readonly List<TodoItem> _items;
        public IReadOnlyCollection<TodoItem> Items => _items.AsReadOnly();

        protected readonly List<TodoSubList> _subLists;
        public IReadOnlyCollection<TodoSubList> SubLists => _subLists.AsReadOnly();

        private TodoList()
        {

        }

        public TodoList(string ownerId, string title, string description = null)
        {
            Guard.Against.NullOrWhiteSpace(ownerId, nameof(ownerId));
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            OwnerId = ownerId;
            Title = title;
            Description = description;

            _items = new List<TodoItem>();
            _subLists = new List<TodoSubList>();
        }

        public void AddSubList(string title, string description = null)
        {
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            var subList = new TodoSubList(title, GetNextSubListOrdinal(), description);
            _subLists.Add(subList);
        }

        public void EditSubList(int subListId, string title, string description = null)
        {
            Guard.Against.NegativeOrZero(subListId, nameof(subListId));
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            var subList = SubLists.SingleOrDefault(sl => sl.Id == subListId);

            if (subList == null)
                throw new TodoSubListDoesNotExistException(Id, subListId);

            if (subList.IsDeleted)
                throw new TodoSubListDeletedException(Id, subListId);

            subList.Edit(title, description);
        }

        public void MoveSubList(int subListId, int newOrdinal)
        {
            Guard.Against.NegativeOrZero(subListId, nameof(subListId));
            Guard.Against.OutOfRange(newOrdinal, nameof(newOrdinal), 1, SubLists.Count);

            var targetSubList = SubLists.SingleOrDefault(sl => sl.Id == subListId);

            if (targetSubList == null)
                throw new TodoSubListDoesNotExistException(Id, subListId);

            if (targetSubList.IsDeleted)
                throw new TodoSubListDeletedException(Id, subListId);

            if (targetSubList.Ordinal == newOrdinal)
                return;

            var listTransformationStartOrdinal = Math.Min(targetSubList.Ordinal, newOrdinal);
            var listTransformationEndOrdinal = Math.Max(targetSubList.Ordinal, newOrdinal);

            var isMovingUp = targetSubList.Ordinal < newOrdinal;

            var otherSubLists = SubLists
                .Where(sl =>
                    sl.Id != subListId && !sl.IsDeleted && sl.Ordinal >= listTransformationStartOrdinal &&
                    sl.Ordinal <= listTransformationEndOrdinal).OrderBy(sl => sl.Ordinal).ToList();

            for (int i = 0; i < otherSubLists.Count; i++)
            {
                var newCalculatedOrdinal =
                    isMovingUp
                        ? i + listTransformationStartOrdinal
                        : i + listTransformationStartOrdinal + 1;

                otherSubLists[i].SetOrdinal(newCalculatedOrdinal);
            }

            targetSubList.SetOrdinal(newOrdinal);
        }

        public void DeleteSubList(int subListId)
        {
            Guard.Against.NegativeOrZero(subListId, nameof(subListId));

            var subList = SubLists.SingleOrDefault(sl => sl.Id == subListId);

            if (subList == null)
                throw new TodoSubListDoesNotExistException(Id, subListId);

            if (subList.IsDeleted)
                return;

            subList.Delete();
        }

        public void AddTodo(string title, string description = null, DateTime? dueDate = null, int? subListId = null)
        {
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            if (subListId.HasValue)
            {
                Guard.Against.NegativeOrZero(subListId.Value, nameof(subListId));

                var subList = SubLists.SingleOrDefault(sl => sl.Id == subListId.Value);

                if (subList == null)
                    throw new TodoSubListDoesNotExistException(Id, subListId.Value);

                if (subList.IsDeleted)
                    throw new TodoSubListDeletedException(Id, subListId.Value);
            }

            var todo = new TodoItem(Id, title, new TodoItemPosition(GetNextItemOrdinal(subListId), subListId), description, dueDate);
            _items.Add(todo);
        }

        public void EditTodo(int todoId, string title, string description = null, DateTime? dueDate = null)
        {
            Guard.Against.NegativeOrZero(todoId, nameof(todoId));
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            var todo = GetTodoById(todoId);

            if (todo == null)
                throw new TodoItemDoesNotExistException(Id, todoId);

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

            todo.Edit(title, description, dueDate);
        }

        public void SetCompletedTodo(int todoId, bool isCompleted = true)
        {
            Guard.Against.NegativeOrZero(todoId, nameof(todoId));

            var todo = GetTodoById(todoId);

            if (todo == null)
                throw new TodoItemDoesNotExistException(Id, todoId);

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
            Guard.Against.NegativeOrZero(todoId, nameof(todoId));
            
            if (newPosition.SubListId.HasValue)
            {
                var subList = SubLists.SingleOrDefault(sl => sl.Id == newPosition.SubListId.Value);

                if (subList == null)
                    throw new TodoSubListDoesNotExistException(Id, newPosition.SubListId.Value);

                if (subList.IsDeleted)
                    throw new TodoSubListDeletedException(Id, newPosition.SubListId.Value);
            }

            Guard.Against.OutOfRange(newPosition.Ordinal, nameof(newPosition), 1,
                Items.Count(item => !item.IsDeleted && item.Position.SubListId == newPosition.SubListId));

            var todoToBeMoved = Items.SingleOrDefault(item => item.Id == todoId);

            if (todoToBeMoved == null)
                throw new TodoItemDoesNotExistException(Id, todoId);

            if (todoToBeMoved.Position.SubListId.HasValue)
            {
                var sourceSubList = SubLists.Single(sl => sl.Id == todoToBeMoved.Position.SubListId.Value);

                if(sourceSubList.IsDeleted)
                    throw new TodoSubListDeletedException(Id, todoToBeMoved.Position.SubListId.Value);
            }

            if (todoToBeMoved.IsDeleted)
                throw new TodoItemDeletedException(Id, todoId);

            var sourceSubListId = todoToBeMoved.Position.SubListId;
            var destinationSubListId = newPosition.SubListId;

            if (sourceSubListId == destinationSubListId)
            {
                var listTransformationStartOrdinal = Math.Min(todoToBeMoved.Position.Ordinal, newPosition.Ordinal);
                var listTransformationEndOrdinal = Math.Max(todoToBeMoved.Position.Ordinal, newPosition.Ordinal);

                var isMovingUp = todoToBeMoved.Position.Ordinal < newPosition.Ordinal;

                var otherItems = Items.Where(item =>
                    item.Position.SubListId == destinationSubListId && !item.IsDeleted && item.Id != todoId &&
                    item.Position.Ordinal >= listTransformationStartOrdinal &&
                    item.Position.Ordinal <= listTransformationEndOrdinal).ToList();

                for (int i = 0; i < otherItems.Count; i++)
                {
                    var newCalculatedOrdinal = isMovingUp
                        ? i + listTransformationStartOrdinal
                        : i + listTransformationStartOrdinal + 1;

                    otherItems[i].SetPosition(new TodoItemPosition(newCalculatedOrdinal, destinationSubListId));
                }
            }
            else
            {
                var sourceListOtherItems = Items.Where(item =>
                    item.Position.SubListId == sourceSubListId && !item.IsDeleted && item.Id != todoId &&
                    item.Position.Ordinal > todoToBeMoved.Position.Ordinal).ToList();

                for (int i = 0; i < sourceListOtherItems.Count; i++)
                {
                    sourceListOtherItems[i].SetPosition(new TodoItemPosition(i + todoToBeMoved.Position.Ordinal, sourceSubListId));
                }

                var destinationListOtherMovedItems = Items.Where(item =>
                    item.Position.SubListId == destinationSubListId && !item.IsDeleted &&
                    item.Position.Ordinal >= newPosition.Ordinal).ToList();

                for (int i = 0; i < destinationListOtherMovedItems.Count; i++)
                {
                    destinationListOtherMovedItems[i].SetPosition(new TodoItemPosition(i + newPosition.Ordinal + 1, destinationSubListId));
                }
            }

            todoToBeMoved.SetPosition(newPosition);
        }

        public void DeleteTodo(int todoId)
        {
            Guard.Against.NegativeOrZero(todoId, nameof(todoId));

            var todo = GetTodoById(todoId);

            if (todo == null)
                throw new TodoItemDoesNotExistException(Id, todoId);

            var subListId = todo.Position.SubListId;

            if (subListId.HasValue)
            {
                if (SubLists.Single(sl => sl.Id == subListId.Value).IsDeleted)
                    throw new TodoSubListDeletedException(Id, subListId.Value);
            }

            if (todo.IsDeleted)
                return;

            todo.Delete();
        }

        private TodoItem GetTodoById(int todoId)
        {
            return Items.SingleOrDefault(item => item.Id == todoId);
        }

        private int GetNextItemOrdinal(int? subListId = null)
        {
            return Items.Count(item => item.Position.SubListId == subListId) + 1;
        }

        private int GetNextSubListOrdinal()
        {
            return SubLists.Count + 1;
        }
    }
}
