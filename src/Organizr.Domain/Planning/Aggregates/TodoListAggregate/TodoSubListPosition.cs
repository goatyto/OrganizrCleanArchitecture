using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.TodoListAggregate
{
    public class TodoSubListPosition : ValueObject, IComparable<TodoSubListPosition>, IComparable<int>
    {
        public int Ordinal { get; private set; }

        public TodoSubListPosition(int ordinal)
        {
            Guard.Against.NegativeOrZero(ordinal, nameof(ordinal));

            Ordinal = ordinal;
        }


        public static TodoSubListPosition operator +(TodoSubListPosition position, int value) => new TodoSubListPosition(position.Ordinal + value);
        public static TodoSubListPosition operator -(TodoSubListPosition position, int value) => new TodoSubListPosition(position.Ordinal - value);

        public static TodoSubListPosition operator +(int value, TodoSubListPosition position) => new TodoSubListPosition(position.Ordinal + value);
        public static TodoSubListPosition operator -(int value, TodoSubListPosition position) => new TodoSubListPosition(position.Ordinal - value);

        public static bool operator >(TodoSubListPosition a, TodoSubListPosition b) => a.Ordinal > b.Ordinal;
        public static bool operator <(TodoSubListPosition a, TodoSubListPosition b) => a.Ordinal < b.Ordinal;

        public static bool operator >=(TodoSubListPosition a, TodoSubListPosition b) => a.Ordinal >= b.Ordinal;
        public static bool operator <=(TodoSubListPosition a, TodoSubListPosition b) => a.Ordinal <= b.Ordinal;

        public static bool operator >(TodoSubListPosition position, int value) => position.Ordinal > value;
        public static bool operator <(TodoSubListPosition position, int value) => position.Ordinal < value;

        public static bool operator >=(TodoSubListPosition position, int value) => position.Ordinal >= value;
        public static bool operator <=(TodoSubListPosition position, int value) => position.Ordinal <= value;

        //public static bool operator >(int value, TodoSubListPosition position) => position.Ordinal > value;
        //public static bool operator <(int value, TodoSubListPosition position) => position.Ordinal < value;

        //public static bool operator >=(int value, TodoSubListPosition position) => position.Ordinal >= value;
        //public static bool operator <=(int value, TodoSubListPosition position) => position.Ordinal <= value;

        //public static bool operator ==(TodoSubListPosition position, int value) => position.Ordinal == value;
        //public static bool operator !=(TodoSubListPosition position, int value) => position.Ordinal != value;

        //public static bool operator ==(int value, TodoSubListPosition position) => position.Ordinal == value;
        //public static bool operator !=(int value, TodoSubListPosition position) => position.Ordinal != value;

        public static explicit operator TodoSubListPosition(int ordinal) => new TodoSubListPosition(ordinal);

        public int CompareTo(TodoSubListPosition other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Ordinal.CompareTo(other.Ordinal);
        }

        public int CompareTo(int value)
        {
            return Ordinal.CompareTo(value);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Ordinal;
        }
    }
}
