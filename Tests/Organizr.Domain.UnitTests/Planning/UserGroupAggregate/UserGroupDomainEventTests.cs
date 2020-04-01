using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
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
            var userGroup = UserGroup.Create(Guid.NewGuid(), "User1", "Name", "Description");

            var sut = new UserGroupCreated(userGroup);

            sut.UserGroup.Should().Be(userGroup);
        }

        [Fact]
        public void UserGroupCreatedConstructor_NullUserGroup_ThrowsArgumentException()
        {
            UserGroup nullUserGroup =  null;

            Func<UserGroupCreated> sut = () => new UserGroupCreated(nullUserGroup);

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("userGroup");
        }

        [Fact]
        public void UserGroupEditedConstructor_ValidData_ObjectInitialized()
        {
            var userGroup = UserGroup.Create(Guid.NewGuid(), "User1", "Name", "Description");

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
            var userGroupId = Guid.NewGuid();
            var userId = "User1";

            var sut = new UserGroupMemberAdded(userGroupId, userId);

            sut.UserGroupId.Should().Be(userGroupId);
            sut.UserId.Should().Be(userId);
        }

        [Fact]
        public void UserGroupMemberAddedConstructor_DefaultUserGroupId_ThrowsArgumentException()
        {
            var defaultUserGroupId = Guid.Empty;

            Func<UserGroupMemberAdded> sut = () => new UserGroupMemberAdded(defaultUserGroupId, "User1");

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("userGroupId");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void UserGroupMemberAddedConstructor_NullOrWhiteSpaceUserId_ThrowsArgumentException(string nullOrWhiteSpaceUserId)
        {
            Func<UserGroupMemberAdded> sut = () => new UserGroupMemberAdded(Guid.NewGuid(), nullOrWhiteSpaceUserId);

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("userId");
        }

        [Fact]
        public void UserGroupMemberRemovedConstructor_ValidData_ObjectInitialized()
        {
            var userGroupId = Guid.NewGuid();
            var userId = "User1";

            var sut = new UserGroupMemberRemoved(userGroupId, userId);

            sut.UserGroupId.Should().Be(userGroupId);
            sut.UserId.Should().Be(userId);
        }

        [Fact]
        public void UserGroupMemberRemovedConstructor_DefaultUserGroupId_ThrowsArgumentException()
        {
            var defaultUserGroupId = Guid.Empty;

            Func<UserGroupMemberRemoved> sut = () => new UserGroupMemberRemoved(defaultUserGroupId, "User1");

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("userGroupId");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void UserGroupMemberRemovedConstructor_NullOrWhiteSpaceUserId_ThrowsArgumentException(string nullOrWhiteSpaceUserId)
        {
            Func<UserGroupMemberRemoved> sut = () => new UserGroupMemberRemoved(Guid.NewGuid(), nullOrWhiteSpaceUserId);

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("userId");
        }
    }
}
