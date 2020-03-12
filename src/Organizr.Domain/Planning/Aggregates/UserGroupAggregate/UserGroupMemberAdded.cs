using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.UserGroupAggregate
{
    public class UserGroupMemberAdded : IDomainEvent
    {
        public Guid UserGroupId { get; }
        public string UserId { get; }

        public UserGroupMemberAdded(Guid userGroupId, string userId)
        {
            Guard.Against.Default(userGroupId, nameof(userGroupId));
            Guard.Against.NullOrWhiteSpace(userId, nameof(userId));
            
            UserGroupId = userGroupId;
            UserId = userId;
        }
    }
}
