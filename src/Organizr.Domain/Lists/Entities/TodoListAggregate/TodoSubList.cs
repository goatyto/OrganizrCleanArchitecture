using System;
using System.Linq;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Lists.Entities.TodoListAggregate
{
    public class TodoSubList: Entity
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
        public int Ordinal { get; protected internal set; }
        public bool IsDeleted { get; protected internal set; }

        private TodoSubList() : base()
        {

        }

        protected internal TodoSubList(string title, int ordinal, string description = null)
        {
            Title = title;
            Ordinal = ordinal;
            Description = description;
        }

        internal void Edit(string title, string description = null)
        {
            Title = title;
            Description = description;
        }

        internal void ChangeOrdinal(int newOrdinal)
        {
            Ordinal = newOrdinal;
        }

        internal void Delete()
        {
            IsDeleted = true;
        }
    }
}
