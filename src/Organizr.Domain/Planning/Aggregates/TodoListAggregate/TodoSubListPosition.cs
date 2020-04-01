using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.TodoListAggregate
{
    public class TodoSubListPosition : CompositeKeyEntity
    {
        public int Ordinal { get; private set; }

        internal TodoSubListPosition(int ordinal)
        {
            Guard.Against.NegativeOrZero(ordinal, nameof(ordinal));

            Ordinal = ordinal;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Ordinal;
        }
    }
}
