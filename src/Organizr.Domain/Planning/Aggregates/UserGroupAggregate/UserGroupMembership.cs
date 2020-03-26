using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.UserGroupAggregate
{
    public class UserGroupMembership : ValueObject
    {
        public string UserId { get; }

        private UserGroupMembership()
        {
            
        }

        public UserGroupMembership(string userId)
        {
            Guard.Against.NullOrWhiteSpace(userId, nameof(userId));

            UserId = userId;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return UserId;
        }

        public static implicit operator string(UserGroupMembership membership) => membership.UserId;
        public static explicit operator UserGroupMembership(string userId) => new UserGroupMembership(userId);
    }
}
