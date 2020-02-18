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
        private string _title;
        public string Title
        {
            get => _title;
            protected internal set
            {
                Guard.Against.NullOrWhiteSpace(value, nameof(Title));
                _title = value;
            }
        }

        public string Description { get; protected internal set; }

        public DateTime? DueDate => Items.Any(item => item.DueDate.HasValue)
            ? Items.Where(item => item.DueDate.HasValue).Max(item => item.DueDate)
            : null;

        protected internal readonly List<TodoItem> _items;
        public IReadOnlyCollection<TodoItem> Items => _items.AsReadOnly();

        protected internal readonly List<TodoSubList> _subLists;
        public IReadOnlyCollection<TodoSubList> SubLists => _subLists.AsReadOnly();

        private TodoList()
        {

        }

        public TodoList(string ownerId, string title, string description = null)
        {
            OwnerId = ownerId;
            Title = title;
            Description = description;

            _items = new List<TodoItem>();
            _subLists = new List<TodoSubList>();
        }

        public void AddSubList(string title, string description = null)
        {
            var subList = new TodoSubList(title, GetNextSubListOrdinal(), description);
            _subLists.Add(subList);
        }

        public void EditSubList(int subListId, string title, string description = null)
        {
            var subList = SubLists.SingleOrDefault(sl => sl.Id == subListId);

            if (subList == null)
                throw new TodoSubListDoesNotExistException(Id, subListId);

            if (subList.IsDeleted)
                throw new TodoSubListDeletedException(Id, subListId);

            subList.Edit(title, description);
        }

        public void DeleteSubList(int subListId)
        {
            var subList = SubLists.SingleOrDefault(sl => sl.Id == subListId);

            if (subList == null)
                throw new TodoSubListDoesNotExistException(Id, subListId);

            if (subList.IsDeleted)
                return;

            subList.Delete();
        }

        public void AddTodo(string title, string description = null, DateTime? dueDate = null, int? subListId = null)
        {
            if (subListId.HasValue)
            {
                if (SubLists.All(sl => sl.Id != subListId.Value))
                    throw new TodoSubListDoesNotExistException(Id, subListId.Value);

                if (SubLists.Single(sl => sl.Id == subListId.Value).IsDeleted)
                    throw new TodoSubListDeletedException(Id, subListId.Value);
            }

            var todo = new TodoItem(Id, title, new TodoItemPosition(GetNextItemOrdinal(subListId), subListId), description, dueDate);
            _items.Add(todo);
        }

        public void EditTodo(int todoId, string title, string description = null, DateTime? dueDate = null)
        {
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

        public void DeleteTodo(int todoId)
        {
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
