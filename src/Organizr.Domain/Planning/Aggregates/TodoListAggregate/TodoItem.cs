using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.TodoListAggregate
{
    public class TodoItem : Entity<int>
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public TodoItemPosition Position { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? DueDateUtc { get; private set; }

        private TodoItem()
        {

        }

        internal TodoItem(int id, string title, TodoItemPosition position, string description = null, DateTime? dueDateUtc = null) : this()
        {
            Id = id;
            Title = title;
            Position = position;
            Description = description;
            DueDateUtc = dueDateUtc;
        }

        internal void Edit(string title, string description = null, DateTime? dueDateUtc = null)
        {
            Title = title;
            Description = description;
            DueDateUtc = dueDateUtc;
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
