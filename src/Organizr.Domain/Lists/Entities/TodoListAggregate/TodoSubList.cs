using System;
using System.Linq;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Lists.Entities.TodoListAggregate
{
    public class TodoSubList: Entity<Guid>
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
        public bool IsDeleted { get; private set; }

        private TodoSubList() : base()
        {

        }

        public TodoSubList(Guid id, string title, string description = null): base(id)
        {
            Id = id;
            Title = title;
            Description = description;
        }

        internal void Edit(string title, string description = null)
        {
            Title = title;
            Description = description;
        }

        internal void Delete()
        {
            IsDeleted = true;
        }
    }
}
