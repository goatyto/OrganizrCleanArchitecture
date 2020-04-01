using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.UserGroupAggregate
{
    public class UserGroupTests
    {
        [Fact]
        public void Create_ValidData_ObjectInitialized()
        {
            var id = Guid.NewGuid();
            var name = "GroupName";
            var creatorUserId = "User1";
            var description = "GroupDescription";
            var memberUserIds = new List<string> {"User1", "User2", "User3"};

            var sut = UserGroup.Create(id, creatorUserId, name, description, memberUserIds);

            sut.Id.Should().Be(id);
            sut.Name.Should().Be(name);
            sut.CreatorUserId.Should().Be(creatorUserId);
            sut.Description.Should().Be(description);
            sut.Membership.Should().NotBeNull().And
                .Contain(memberUserIds.Select(uid => new UserGroupMembership(uid)));
        }

        [Fact]
        public void Create_DefaultId_ThrowsArgumentException()
        {
            var defaultId = Guid.Empty;

            Func<UserGroup> sut = () => UserGroup.Create(defaultId, "User1", "GroupName", "GroupDescription");

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("id");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_NullOrWhiteSpaceCreatorUserId_ThrowsArgumentException(
            string nullOrWhiteSpaceCreatorUserId)
        {
            Func<UserGroup> sut = () =>
                UserGroup.Create(Guid.NewGuid(), nullOrWhiteSpaceCreatorUserId, "GroupName", "GroupDescription");

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("creatorUserId");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_NullOrWhiteSpaceName_ThrowsArgumentException(string nullOrWhiteSpaceName)
        {
            Func<UserGroup> sut = () =>
                UserGroup.Create(Guid.NewGuid(), "User1", nullOrWhiteSpaceName, "GroupDescription");

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("name");
        }

        [Fact]
        public void Edit_ValidData_StateChanges()
        {
            var newName = "NewName";
            var newDescription = "NewDescription";

            var sut = UserGroup.Create(Guid.NewGuid(), "User1", "GroupName", "GroupDescription");

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

            var sut = UserGroup.Create(Guid.NewGuid(), "User1", "GroupName", "GroupDescription");

            sut.Invoking(s => s.Edit(nullOrWhiteSpaceName, newDescription)).Should().Throw<ArgumentException>().And
                .ParamName.Should().Be("name");
        }
    }
}
