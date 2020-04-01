using System;
using System.Collections.Generic;
using System.Text;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.TodoListAggregate
{
    public class TodoSubListId : ValueObject, IComparable<TodoSubListId>, IComparable<int>
    {
        public int Id { get; private set; }

        private TodoSubListId(int id)
        {
            Id = id;
        }

        public static TodoSubListId Create(int? id)
        {
            if (!id.HasValue)
                return null;

            return new TodoSubListId(id.Value);
        }

        public static explicit operator TodoSubListId(int id) => new TodoSubListId(id);

        public int CompareTo(TodoSubListId other)
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
