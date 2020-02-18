using System;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Lists.Entities.TodoListAggregate
{
    public class TodoItem : Entity
    {
        private Guid _mainListId;
        public Guid MainListId
        {
            get => _mainListId;
            protected internal set
            {
                Guard.Against.Default(value, nameof(MainListId));
                _mainListId = value;
            }
        }

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
        public TodoItemPosition Position { get; protected internal set; }
        public bool IsCompleted { get; protected internal set; }
        public bool IsDeleted { get; protected internal set; }
        public DateTime? DueDate { get; protected internal set; }

        private TodoItem()
        {
            
        }

        protected internal TodoItem(Guid mainListId, string title, TodoItemPosition position, string description = null, DateTime? dueDate = null)
        {
            MainListId = mainListId;
            Title = title;
            Position = position;
            Description = description;
            DueDate = dueDate;
        }

        internal void Edit(string title, string description = null, DateTime? dueDate = null)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
        }

        internal void ChangePosition(TodoItemPosition position)
        {
            Position = position;
        }

        internal void SetCompleted(bool isCompleted = true)
        {
            IsCompleted = isCompleted;
        }

        internal void Delete()
        {
            IsDeleted = true;
        }
    }
}
