using System;
using System.Collections.Generic;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.UserGroupAggregate
{
    public class UserGroupDomainEventTests
    {
        [Fact]
        public void UserGroupCreatedConstructor_ValidData_ObjectInitialized()
        {
            var userGroupId = Guid.NewGuid();
            var creatorUserId = "User1";
            var userGroupName = "Name";
            var memberIds = new List<string> { "User2" };
            var userGroupDescription = "Description";

            var userGroup = UserGroup.Create(userGroupId, creatorUserId, userGroupName, memberIds, userGroupDescription);

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
            var userGroupId = Guid.NewGuid();
            var creatorUserId = "User1";
            var userGroupName = "Name";
            var memberIds = new List<string> { "User2" };
            var userGroupDescription = "Description";

            var userGroup = UserGroup.Create(userGroupId, creatorUserId, userGroupName, memberIds, userGroupDescription);

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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void UserGroupMemberAddedConstructor_NullOrWhiteSpaceUserId_ThrowsArgumentException(string nullOrWhiteSpaceUserId)
        {
            var userGroupId = Guid.NewGuid();
            
            Func<UserGroupMemberAdded> sut = () => new UserGroupMemberAdded(userGroupId, nullOrWhiteSpaceUserId);

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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void UserGroupMemberRemovedConstructor_NullOrWhiteSpaceUserId_ThrowsArgumentException(string nullOrWhiteSpaceUserId)
        {
            var userGroupId = Guid.NewGuid();

            Func<UserGroupMemberRemoved> sut = () => new UserGroupMemberRemoved(userGroupId, nullOrWhiteSpaceUserId);

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("userId");
        }
    }
}
