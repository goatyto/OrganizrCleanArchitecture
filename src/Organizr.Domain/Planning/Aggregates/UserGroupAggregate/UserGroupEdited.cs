using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.UserGroupAggregate
{
    public class UserGroupEdited : IDomainEvent
    {
        public UserGroup UserGroup { get; }

        public UserGroupEdited(UserGroup userGroup)
        {
            Assert.Argument.NotNull(userGroup, nameof(userGroup));

            UserGroup = userGroup;
        }
    }
}
