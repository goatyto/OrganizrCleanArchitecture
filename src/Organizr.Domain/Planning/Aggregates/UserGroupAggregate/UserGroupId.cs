using System;
using System.Collections.Generic;
using System.Text;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.UserGroupAggregate
{
    public class UserGroupId : ValueObject
    {
        public Guid Id { get; private set; }

        public UserGroupId(Guid id)
        {
            Id = id;
        }

        public static explicit operator UserGroupId(Guid id) => new UserGroupId(id);

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
        }
    }
}
