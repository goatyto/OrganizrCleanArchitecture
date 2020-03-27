using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.TodoListAggregate
{
    public class TodoItemPosition : ValueObject, IComparable<TodoItemPosition>, IComparable<int>
    {
        public int Ordinal { get; private set; }

        public TodoItemPosition(int ordinal)
        {
            Guard.Against.NegativeOrZero(ordinal, nameof(ordinal));

            Ordinal = ordinal;
        }

        public static TodoItemPosition operator +(TodoItemPosition position, int value) => new TodoItemPosition(position.Ordinal + value);
        public static TodoItemPosition operator -(TodoItemPosition position, int value) => new TodoItemPosition(position.Ordinal - value);

        public static TodoItemPosition operator +(int value, TodoItemPosition position) => new TodoItemPosition(position.Ordinal + value);
        public static TodoItemPosition operator -(int value, TodoItemPosition position) => new TodoItemPosition(position.Ordinal - value);

        public static bool operator >(TodoItemPosition a, TodoItemPosition b) => a.Ordinal > b.Ordinal;
        public static bool operator <(TodoItemPosition a, TodoItemPosition b) => a.Ordinal < b.Ordinal;

        public static bool operator >=(TodoItemPosition a, TodoItemPosition b) => a.Ordinal >= b.Ordinal;
        public static bool operator <=(TodoItemPosition a, TodoItemPosition b) => a.Ordinal <= b.Ordinal;

        public static bool operator >(TodoItemPosition position, int value) => position.Ordinal > value;
        public static bool operator <(TodoItemPosition position, int value) => position.Ordinal < value;

        public static bool operator >=(TodoItemPosition position, int value) => position.Ordinal >= value;
        public static bool operator <=(TodoItemPosition position, int value) => position.Ordinal <= value;

        //public static bool operator >(int value, TodoItemPosition position) => position.Ordinal > value;
        //public static bool operator <(int value, TodoItemPosition position) => position.Ordinal < value;

        //public static bool operator >=(int value, TodoItemPosition position) => position.Ordinal >= value;
        //public static bool operator <=(int value, TodoItemPosition position) => position.Ordinal <= value;

        //public static bool operator ==(TodoItemPosition position, int value) => position.Ordinal == value;
        //public static bool operator !=(TodoItemPosition position, int value) => position.Ordinal != value;

        //public static bool operator ==(int value, TodoItemPosition position) => position.Ordinal == value;
        //public static bool operator !=(int value, TodoItemPosition position) => position.Ordinal != value;

        public static explicit operator TodoItemPosition(int value) => new TodoItemPosition(value);

        public int CompareTo(TodoItemPosition other)
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
