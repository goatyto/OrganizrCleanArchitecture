using System.Collections.Generic;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.UserGroupAggregate
{
    public class UserGroupMember : ValueObject
    {
        public string UserId { get; }

        private UserGroupMember()
        {
            
        }

        internal UserGroupMember(string userId)
        {
            Assert.Argument.NotEmpty(userId, nameof(userId));

            UserId = userId;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return UserId;
        }

        public static implicit operator string(UserGroupMember member) => member.UserId;
        public static explicit operator UserGroupMember(string userId) => new UserGroupMember(userId);
    }
}
