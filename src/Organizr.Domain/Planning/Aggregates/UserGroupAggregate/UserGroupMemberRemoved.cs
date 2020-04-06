using System;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.UserGroupAggregate
{
    class UserGroupMemberRemoved : IDomainEvent
    {
        public Guid UserGroupId { get; }
        public string UserId { get; }

        public UserGroupMemberRemoved(Guid userGroupId, string userId)
        {
            Assert.Argument.NotDefault(userGroupId, nameof(userGroupId));
            Assert.Argument.NotEmpty(userId, nameof(userId));

            UserGroupId = userGroupId;
            UserId = userId;
        }
    }
}
