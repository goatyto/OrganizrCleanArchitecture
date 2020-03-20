using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using Organizr.Domain.Guards;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.TodoListAggregate
{
    public class TodoItem : Entity<int>
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int Ordinal { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? DueDateUtc { get; private set; }
        public int? SubListId { get; private set; }

        private TodoItem()
        {

        }

        internal TodoItem(int id, string title, int ordinal, string description = null, DateTime? dueDateUtc = null, int? subListId = null) : this()
        {
            Id = id;
            Title = title;
            Ordinal = ordinal;
            Description = description;
            DueDateUtc = dueDateUtc;
            SubListId = subListId;
        }

        internal void Edit(string title, string description = null, DateTime? dueDateUtc = null)
        {
            Title = title;
            Description = description;
            DueDateUtc = dueDateUtc;
        }

        internal void SetPosition(int ordinal, int? subListId = null)
        {
            Ordinal = ordinal;
            SubListId = subListId;
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
