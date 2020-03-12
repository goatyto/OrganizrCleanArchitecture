using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.TodoListAggregate
{
    public class TodoSubList : Entity<int>
    {
        public Guid TodoListId { get; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int Ordinal { get; private set; }
        public bool IsDeleted { get; private set; }

        private TodoSubList()
        {

        }

        internal TodoSubList(int id, Guid todoListId, string title, int ordinal, string description = null) : this()
        {
            Id = id;
            TodoListId = todoListId;
            Title = title;
            Ordinal = ordinal;
            Description = description;
        }

        internal void Edit(string title, string description = null)
        {
            Title = title;
            Description = description;
        }

        internal void SetOrdinal(int newOrdinal)
        {
            Ordinal = newOrdinal;
        }

        internal void Delete()
        {
            IsDeleted = true;
        }
    }
}
