using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.UserGroupAggregate
{
    class UserGroupMemberRemoved : IDomainEvent
    {
        public Guid UserGroupId { get; }
        public string UserId { get; }

        public UserGroupMemberRemoved(Guid userGroupId, string userId)
        {
            Guard.Against.Default(userGroupId, nameof(userGroupId));
            Guard.Against.NullOrWhiteSpace(userId, nameof(userId));

            UserGroupId = userGroupId;
            UserId = userId;
        }
    }
}
