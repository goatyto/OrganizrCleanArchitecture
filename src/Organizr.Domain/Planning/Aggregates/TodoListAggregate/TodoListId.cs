﻿using System;
using System.Collections.Generic;
using System.Text;
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

        public static explicit operator TodoListId(Guid id) => new TodoListId(id);

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
        }
    }
}
