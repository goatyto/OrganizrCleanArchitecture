using System;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Lists.Entities.TodoListAggregate
{
    public class TodoItem : Entity<Guid>
    {
        private Guid _mainListId;
        public Guid MainListId
        {
            get => _mainListId;
            private set
            {
                Guard.Against.Default(value, nameof(MainListId));
                _mainListId = value;
            }
        }

        private Guid? _subListId;
        public Guid? SubListId
        {
            get => _subListId;
            private set
            {
                if(value.HasValue)
                    Guard.Against.Default(value.Value, nameof(SubListId));

                _subListId = value;
            }
        }

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
        public int Ordinal { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? DueDate { get; private set; }

        public TodoItem(Guid id, Guid mainListId, string title, int ordinal, string description = null, DateTime? dueDate = null, Guid? subListId = null) : base(id)
        {
            MainListId = mainListId;
            Title = title;
            Ordinal = ordinal;
            Description = description;
            DueDate = dueDate;
            SubListId = subListId;
        }

        internal void Edit(string title, string description = null, DateTime? dueDate = null)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
        }

        internal void ChangeSubList(Guid? subListId, int ordinal)
        {
            SubListId = subListId;
            Ordinal = ordinal;
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
