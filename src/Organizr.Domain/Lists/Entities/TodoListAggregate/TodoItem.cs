using System;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Lists.Entities.TodoListAggregate
{
    public class TodoItem : Entity
    {
        public Guid MainListId { get; protected set;}
        public string Title { get; protected set; }
        public string Description { get; protected set; }
        public TodoItemPosition Position { get; protected set; }
        public bool IsCompleted { get; protected set; }
        public bool IsDeleted { get; protected set; }
        public DateTime? DueDate { get; protected set; }

        private TodoItem()
        {
            
        }

        protected internal TodoItem(Guid mainListId, string title, TodoItemPosition position, string description = null, DateTime? dueDate = null)
        {
            Guard.Against.Default(mainListId, nameof(mainListId));
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            MainListId = mainListId;
            Title = title;
            Position = position;
            Description = description;
            DueDate = dueDate;
        }

        internal void Edit(string title, string description = null, DateTime? dueDate = null)
        {
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            Title = title;
            Description = description;
            DueDate = dueDate;
        }

        internal void SetPosition(TodoItemPosition position)
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
