using System;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.UserGroupAggregate
{
    public class UserGroupMemberAdded : IDomainEvent
    {
        public Guid UserGroupId { get; }
        public string UserId { get; }

        public UserGroupMemberAdded(Guid userGroupId, string userId)
        {
            Assert.Argument.NotDefault(userGroupId, nameof(userGroupId));
            Assert.Argument.NotEmpty(userId, nameof(userId));
            
            UserGroupId = userGroupId;
            UserId = userId;
        }
    }
}
