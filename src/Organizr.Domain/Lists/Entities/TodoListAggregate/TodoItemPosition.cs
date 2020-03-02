using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Lists.Entities.TodoListAggregate
{
    public class TodoItemPosition : ValueObject
    {
        public int Ordinal { get; }
        public int? SubListId { get; }

        public TodoItemPosition(int ordinal, int? subListId)
        {
            Guard.Against.NegativeOrZero(ordinal, nameof(ordinal));

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
