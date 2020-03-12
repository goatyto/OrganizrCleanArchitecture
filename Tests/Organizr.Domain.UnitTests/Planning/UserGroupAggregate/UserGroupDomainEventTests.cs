using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.UserGroupAggregate
{
    public class UserGroupDomainEventTests
    {
        [Fact]
        public void SharedTodoListCreatedConstructor_ValidData_ObjectInitialized()
        {
            var userGroupId = Guid.NewGuid();
            var todoListId = Guid.NewGuid();

            var sut = new SharedTodoListCreated(userGroupId, todoListId);

            sut.UserGroupId.Should().Be(userGroupId);
            sut.TodoListId.Should().Be(todoListId);
        }

        [Fact]
        public void SharedTodoListCreatedConstructor_DefaultUserGroupId_ThrowsArgumentException()
        {
            var defaultUserGroupId = Guid.Empty;

            Func<SharedTodoListCreated> sut = () => new SharedTodoListCreated(defaultUserGroupId, Guid.NewGuid());

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("userGroupId");
        }

        [Fact]
        public void SharedTodoListCreatedConstructor_DefaultTodoListId_ThrowsArgumentException()
        {
            var defaultTodoListId = Guid.Empty;

            Func<SharedTodoListCreated> sut = () => new SharedTodoListCreated(Guid.NewGuid(), defaultTodoListId);

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("todoListId");
        }

        [Fact]
        public void SharedTodoListDeletedConstructor_ValidData_ObjectInitialized()
        {
            var userGroupId = Guid.NewGuid();
            var todoListId = Guid.NewGuid();

            var sut = new SharedTodoListDeleted(userGroupId, todoListId);

            sut.UserGroupId.Should().Be(userGroupId);
            sut.TodoListId.Should().Be(todoListId);
        }

        [Fact]
        public void SharedTodoListDeletedConstructor_DefaultUserGroupId_ThrowsArgumentException()
        {
            var defaultUserGroupId = Guid.Empty;

            Func<SharedTodoListDeleted> sut = () => new SharedTodoListDeleted(defaultUserGroupId, Guid.NewGuid());

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("userGroupId");
        }

        [Fact]
        public void SharedTodoListDeletedConstructor_DefaultTodoListId_ThrowsArgumentException()
        {
            var defaultTodoListId = Guid.Empty;

            Func<SharedTodoListDeleted> sut = () => new SharedTodoListDeleted(Guid.NewGuid(), defaultTodoListId);

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("todoListId");
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
