using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.UserGroupAggregate
{
    public class UserGroupTests
    {
        [Fact]
        public void Constructor_ValidData_ObjectInitialized()
        {
            var id = Guid.NewGuid();
            string name = "GroupName";
            string creatorUserId = "User1";
            string description = "GroupDescription";

            var sut = new UserGroup(id, creatorUserId, name, description);

            sut.Id.Should().Be(id);
            sut.Name.Should().Be(name);
            sut.CreatorUserId.Should().Be(creatorUserId);
            sut.Description.Should().Be(description);
            sut.Membership.Should().NotBeNull().And.Contain(membership => membership.UserId == creatorUserId);
            sut.SharedTodoLists.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_DefaultId_ThrowsArgumentException()
        {
            var defaultId = Guid.Empty;

            Func<UserGroup> sut = () => new UserGroup(defaultId, "User1", "GroupName", "GroupDescription");

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("id");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_NullOrWhiteSpaceCreatorUserId_ThrowsArgumentException(
            string nullOrWhiteSpaceCreatorUserId)
        {
            Func<UserGroup> sut = () =>
                new UserGroup(Guid.NewGuid(), nullOrWhiteSpaceCreatorUserId, "GroupName", "GroupDescription");

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("creatorUserId");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_NullOrWhiteSpaceName_ThrowsArgumentException(string nullOrWhiteSpaceName)
        {
            Func<UserGroup> sut = () =>
                new UserGroup(Guid.NewGuid(), "User1", nullOrWhiteSpaceName, "GroupDescription");

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("name");
        }

        [Fact]
        public void Edit_ValidData_StateChanges()
        {
            var newName = "NewName";
            var newDescription = "NewDescription";

            var sut = new UserGroup(Guid.NewGuid(), "User1", "GroupName", "GroupDescription");

            sut.Edit(newName, newDescription);

            sut.Name.Should().Be(newName);
            sut.Description.Should().Be(newDescription);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Edit_NullOrWhiteSpaceName_ThrowsArgumentException(string nullOrWhiteSpaceName)
        {
            var newDescription = "NewDescription";

            var sut = new UserGroup(Guid.NewGuid(), "User1", "GroupName", "GroupDescription");

            sut.Invoking(s => s.Edit(nullOrWhiteSpaceName, newDescription)).Should().Throw<ArgumentException>().And
                .ParamName.Should().Be("name");
        }
    }
}
