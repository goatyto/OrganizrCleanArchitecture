using System;
using System.Collections.Generic;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.UserGroupAggregate
{
    public class UserGroupMembership : ValueObject
    {
        public Guid UserGroupId { get; }
        public string UserId { get; }

        public UserGroupMembership(Guid userGroupId, string userId)
        {
            UserGroupId = userGroupId;
            UserId = userId;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return UserGroupId;
            yield return UserId;
        }
    }
}
