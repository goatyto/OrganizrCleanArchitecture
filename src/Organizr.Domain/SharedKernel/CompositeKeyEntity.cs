using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Organizr.Domain.SharedKernel
{
    public abstract class CompositeKeyEntity
    {
        int? _requestedHashCode;

        protected abstract IEnumerable<object> GetAtomicValues();

        public override bool Equals(object obj)
        {
            if (!(obj is CompositeKeyEntity))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            CompositeKeyEntity item = (CompositeKeyEntity)obj;

            var thisValues = GetAtomicValues().GetEnumerator();
            var otherValues = item.GetAtomicValues().GetEnumerator();

            while (thisValues.MoveNext() | otherValues.MoveNext())
            {
                if (ReferenceEquals(thisValues.Current, null) ^ ReferenceEquals(otherValues.Current, null))
                {
                    return false;
                }
                if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            if (!_requestedHashCode.HasValue)
                _requestedHashCode = GetAtomicValues().Select(val => val.GetHashCode())
                    .Aggregate((aggregate, hash) => aggregate ^ hash); // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

            return _requestedHashCode.Value;

        }

        public static bool operator ==(CompositeKeyEntity left, CompositeKeyEntity right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(CompositeKeyEntity left, CompositeKeyEntity right)
        {
            return !(left == right);
        }
    }
}
