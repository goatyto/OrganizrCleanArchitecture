using System;
using System.Collections.Generic;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;

namespace Organizr.Domain.UnitTests.Planning.UserGroupAggregate
{
    public class UserGroupFixture
    {
        public Guid UserGroupId { get; }
        public string UserGroupCreatorId { get; }
        public string ExistingUserGroupMemberId { get; }
        public Guid SharedTodoListId { get; }
        public UserGroup Sut { get; }

        public UserGroupFixture()
        {
            UserGroupId = Guid.NewGuid();
            UserGroupCreatorId = "User1";
            ExistingUserGroupMemberId = "User2";
            SharedTodoListId = Guid.NewGuid();

            Sut = UserGroup.Create(UserGroupId, UserGroupCreatorId, "UserGroupName",
                new List<string> {ExistingUserGroupMemberId}, "UserGroupDescription");

            Sut.CreateSharedTodoList(SharedTodoListId, UserGroupCreatorId, "Title", "Description");
        }
    }
}
