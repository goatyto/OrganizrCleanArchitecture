using System;
using System.Collections.Generic;
using System.Text;
using Organizr.Domain.Planning;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;

namespace Organizr.Domain.UnitTests.Planning.UserGroupAggregate
{
    public class UserGroupFixture
    {
        public UserGroupId UserGroupId { get; }
        public CreatorUser UserGroupCreator { get; }
        public UserGroupMember ExistingUserGroupMember { get; }
        public TodoListId SharedTodoListId { get; }
        public UserGroup Sut { get; }

        public UserGroupFixture()
        {
            UserGroupId = new UserGroupId(Guid.NewGuid());
            UserGroupCreator = new CreatorUser("User1");
            ExistingUserGroupMember = new UserGroupMember("User2");
            SharedTodoListId = new TodoListId(Guid.NewGuid());

            Sut = UserGroup.Create(UserGroupId, UserGroupCreator, "UserGroupName", "UserGroupDescription");

            Sut.AddMember(ExistingUserGroupMember);

            Sut.CreateSharedTodoList(SharedTodoListId, UserGroupCreator, "Title", "Description");
        }
    }
}
