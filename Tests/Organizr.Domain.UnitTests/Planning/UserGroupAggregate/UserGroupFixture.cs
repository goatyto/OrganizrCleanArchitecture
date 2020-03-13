using System;
using System.Collections.Generic;
using System.Text;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;

namespace Organizr.Domain.UnitTests.Planning.UserGroupAggregate
{
    public class UserGroupFixture
    {
        public readonly Guid UserGroupId;
        public readonly string UserGroupCreatorId;
        public readonly string ExistingUserGroupMemberId;
        public readonly Guid SharedTodoListId;
        public readonly UserGroup Sut;

        public UserGroupFixture()
        {
            UserGroupId = Guid.NewGuid();
            UserGroupCreatorId = "User1";
            ExistingUserGroupMemberId = "User2";
            SharedTodoListId = Guid.NewGuid();

            Sut = UserGroup.Create(UserGroupId, UserGroupCreatorId, "UserGroupName", "UserGroupDescription");

            Sut.AddMember(ExistingUserGroupMemberId);

            Sut.CreateSharedTodoList(SharedTodoListId, UserGroupCreatorId, "Title", "Description");
        }
    }
}
