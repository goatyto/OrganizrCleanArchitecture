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
            private set
            {
                Guard.Against.NullOrWhiteSpace(value, nameof(Title));
                _title = value;
            }
        }

        public string Description { get; private set; }
        protected int NextOrdinal => Items.Count(item => !item.IsDeleted) + 1;

        public DateTime? DueDate => Items.Any(item => item.DueDate.HasValue)
            ? Items.Where(item => item.DueDate.HasValue).Max(item => item.DueDate)
            : null;

        private List<TodoItem> _items;
        public IReadOnlyCollection<TodoItem> Items => _items.AsReadOnly();

        private List<TodoSubList> _subLists;
        public IReadOnlyCollection<TodoSubList> SubLists => _subLists.AsReadOnly();

        private TodoList(): base()
        {

        }

        public TodoList(Guid id, string ownerId, string title, string description = null) : base(id, ownerId)
        {
            Id = id;
            OwnerId = ownerId;
            Title = title;
            Description = description;

            _items = new List<TodoItem>();
            _subLists = new List<TodoSubList>();
        }

        public void AddSubList(Guid subListId, string title, string description = null)
        {
            if (SubLists.Any(sublist => sublist.Id == subListId))
                throw new TodoSubListAlreadyExistsException(Id, subListId);

            var subList = new TodoSubList(subListId, title, description);
            _subLists.Add(subList);
        }

        public void EditSubList(Guid subListId, string title, string description = null)
        {
            var subList = SubLists.SingleOrDefault(sl => sl.Id == subListId);

            if (subList == null)
                throw new TodoSubListDoesNotExistException(Id, subListId);

            subList.Edit(title, description);
        }

        public void DeleteSubList(Guid subListId)
        {
            var subList = SubLists.SingleOrDefault(sl => sl.Id == subListId);

            if (subList == null)
                throw new TodoSubListDoesNotExistException(Id, subListId);

            subList.Delete();
        }

        public void AddTodo(Guid todoId, string title, string description = null, DateTime? dueDate = null, Guid? subListId = null)
        {
            if (subListId.HasValue && SubLists.All(sl => sl.Id != subListId.Value))
                throw new TodoSubListDoesNotExistException(Id, subListId.Value);

            var todo = new TodoItem(todoId, Id, title, GetNextOrdinal(subListId), description, dueDate, subListId);
            _items.Add(todo);
        }

        public void EditTodo(Guid todoId, string title, string description = null, DateTime? dueDate = null)
        {
            var todo = GetTodoById(todoId);

            if (todo == null)
                throw new TodoItemDoesNotExistException(Id, todoId);

            todo.Edit(title, description, dueDate);
        }

        public void SetCompletedTodo(Guid todoId, bool isCompleted = true)
        {
            var todo = GetTodoById(todoId);

            if (todo == null)
                throw new TodoItemDoesNotExistException(Id, todoId);

            todo.SetCompleted(isCompleted);
        }

        public void DeleteTodo(Guid todoId)
        {
            var todo = GetTodoById(todoId);

            if (todo == null)
                throw new TodoItemDoesNotExistException(Id, todoId);

            todo.Delete();
        }

        private TodoItem GetTodoById(Guid todoId)
        {
            return Items.SingleOrDefault(item => item.Id == todoId);
        }

        private int GetNextOrdinal(Guid? subListId = null)
        {
            return Items.Count(item => !item.IsDeleted && item.SubListId == subListId) + 1;
        }
    }
}
