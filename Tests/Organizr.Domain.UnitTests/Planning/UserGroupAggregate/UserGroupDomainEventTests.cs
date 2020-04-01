using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Organizr.Domain.Planning;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.UserGroupAggregate
{
    public class UserGroupDomainEventTests
    {
        [Fact]
        public void UserGroupCreatedConstructor_ValidData_ObjectInitialized()
        {
            var userGroup = UserGroup.Create(new UserGroupId(Guid.NewGuid()), new CreatorUser("User1"), "Name", "Description");

            var sut = new UserGroupCreated(userGroup);

            sut.UserGroup.Should().Be(userGroup);
        }

        [Fact]
        public void UserGroupCreatedConstructor_NullUserGroup_ThrowsArgumentException()
        {
            UserGroup nullUserGroup = null;

            Func<UserGroupCreated> sut = () => new UserGroupCreated(nullUserGroup);

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("userGroup");
        }

        [Fact]
        public void UserGroupEditedConstructor_ValidData_ObjectInitialized()
        {
            var userGroup = UserGroup.Create(new UserGroupId(Guid.NewGuid()), new CreatorUser("User1"), "Name", "Description");

            var sut = new UserGroupEdited(userGroup);

            sut.UserGroup.Should().Be(userGroup);
        }

        [Fact]
        public void UserGroupEditedConstructor_NullUserGroup_ThrowsArgumentException()
        {
            UserGroup nullUserGroup = null;

            Func<UserGroupEdited> sut = () => new UserGroupEdited(nullUserGroup);

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("userGroup");
        }

        [Fact]
        public void UserGroupMemberAddedConstructor_ValidData_ObjectInitialized()
        {
            var userGroupId = new UserGroupId(Guid.NewGuid());
            var userId = new UserGroupMember("User1");

            var sut = new UserGroupMemberAdded(userGroupId, userId);

            sut.UserGroupId.Should().Be(userGroupId);
            sut.UserGroupMember.Should().Be(userId);
        }

        [Fact]
        public void UserGroupMemberRemovedConstructor_ValidData_ObjectInitialized()
        {
            var userGroupId = new UserGroupId(Guid.NewGuid());
            var userId = new UserGroupMember("User1");

            var sut = new UserGroupMemberRemoved(userGroupId, userId);

            sut.UserGroupId.Should().Be(userGroupId);
            sut.UserGroupMember.Should().Be(userId);
        }
    }
}
