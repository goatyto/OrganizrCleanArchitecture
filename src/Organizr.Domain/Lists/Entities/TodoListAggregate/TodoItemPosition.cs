using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Lists.Entities.TodoListAggregate
{
    public class TodoItemPosition:ValueObject
    {
        private int _ordinal;
        public int Ordinal
        {
            get => _ordinal;
            protected internal set
            {
                Guard.Against.Default(value, nameof(Ordinal));

                _ordinal = value;
            }
        }
        
        private int? _subListId;
        public int? SubListId
        {
            get => _subListId;
            protected internal set
            {
                if (value.HasValue)
                    Guard.Against.Default(value.Value, nameof(SubListId));

                _subListId = value;
            }
        }

        public TodoItemPosition(int ordinal, int? subListId = null)
        {
            Ordinal = ordinal;
            SubListId = subListId;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Ordinal;
            yield return SubListId;
        }
    }
}
