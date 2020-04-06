using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.UserGroupAggregate
{
    public class UserGroupCreated : IDomainEvent
    {
        public UserGroup UserGroup { get; }

        public UserGroupCreated(UserGroup userGroup)
        {
            Assert.Argument.NotNull(userGroup, nameof(userGroup));

            UserGroup = userGroup;
        }
    }
}
