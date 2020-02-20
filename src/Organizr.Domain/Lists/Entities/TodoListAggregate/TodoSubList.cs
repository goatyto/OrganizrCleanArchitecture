using System;
using System.Linq;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Lists.Entities.TodoListAggregate
{
    public class TodoSubList : Entity
    {
        public string Title { get; protected set; }
        public string Description { get; protected set; }
        public int Ordinal { get; protected set; }
        public bool IsDeleted { get; protected set; }

        private TodoSubList()
        {

        }

        protected internal TodoSubList(string title, int ordinal, string description = null) : this()
        {
            Guard.Against.NullOrWhiteSpace(title, nameof(title));
            Guard.Against.NegativeOrZero(ordinal, nameof(ordinal));

            Title = title;
            Ordinal = ordinal;
            Description = description;
        }

        internal void Edit(string title, string description = null)
        {
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            Title = title;
            Description = description;
        }

        internal void SetOrdinal(int newOrdinal)
        {
            Guard.Against.NegativeOrZero(newOrdinal, nameof(newOrdinal));

            Ordinal = newOrdinal;
        }

        internal void Delete()
        {
            IsDeleted = true;
        }
    }
}
