using System.Collections.Generic;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.TodoListAggregate
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
