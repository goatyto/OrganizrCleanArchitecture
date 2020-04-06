using System;
using System.Collections.Generic;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.TodoListAggregate
{
    public class TodoListId : ValueObject
    {
        public Guid Id { get; private set; }

        public TodoListId(Guid id)
        {
            Id = id;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
