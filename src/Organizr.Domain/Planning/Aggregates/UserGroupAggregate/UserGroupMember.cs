using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.UserGroupAggregate
{
    public class UserGroupMember : ValueObject
    {
        public string UserId { get; }

        private UserGroupMember()
        {

        }

        public UserGroupMember(string userId)
        {
            Guard.Against.NullOrWhiteSpace(userId, nameof(userId));

            UserId = userId;
        }

        public static explicit operator UserGroupMember(CreatorUser creatorUser) => new UserGroupMember(creatorUser.UserId);
        public static explicit operator UserGroupMember(string userId) => new UserGroupMember(userId);

        public static bool operator ==(UserGroupMember userGroupMember, CreatorUser creatorUser) =>
            userGroupMember?.UserId == creatorUser?.UserId;
        public static bool operator !=(UserGroupMember userGroupMember, CreatorUser creatorUser) => !(userGroupMember == creatorUser);

        public static bool operator ==(CreatorUser creatorUser, UserGroupMember userGroupMember) =>
            userGroupMember?.UserId == creatorUser?.UserId;
        public static bool operator !=(CreatorUser creatorUser, UserGroupMember userGroupMember) => !(userGroupMember == creatorUser);

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return UserId;
        }

        public override string ToString()
        {
            return UserId;
        }
    }
}
