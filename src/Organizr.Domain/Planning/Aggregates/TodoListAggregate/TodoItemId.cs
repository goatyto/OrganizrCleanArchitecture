using System;
using System.Collections.Generic;
using System.Text;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.TodoListAggregate
{
    public class TodoItemId : ValueObject, IComparable<TodoItemId>, IComparable<int>
    {
        public int Id { get; private set; }

        public TodoItemId(int id)
        {
            Id = id;
        }

        public static explicit operator TodoItemId(int id) => new TodoItemId(id);

        public int CompareTo(TodoItemId other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Id.CompareTo(other.Id);
        }

        public int CompareTo(int value)
        {
            return Id.CompareTo(value);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
