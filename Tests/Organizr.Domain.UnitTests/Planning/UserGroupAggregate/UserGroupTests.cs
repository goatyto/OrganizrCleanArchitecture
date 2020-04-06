using System;
using System.Collections.Generic;
using System.Linq;
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
            var memberUserIds = new List<string> { "User1", "User2", "User3" };

            var sut = UserGroup.Create(id, creatorUserId, name, memberUserIds, description);

            sut.Id.Should().Be(id);
            sut.Name.Should().Be(name);
            sut.CreatorUserId.Should().Be(creatorUserId);
            sut.Description.Should().Be(description);
            sut.Members.Should().NotBeNull().And
                .Contain(memberUserIds.Select(uid => new UserGroupMember(uid)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_NullOrWhiteSpaceCreatorUserId_ThrowsArgumentException(string nullOrWhiteSpaceCreatorUserId)
        {
            var userGroupId = Guid.NewGuid();
            var userGroupName = "GroupName";
            var userGroupDescription = "GroupDescription";

            Func<UserGroup> sut = () => UserGroup.Create(userGroupId, nullOrWhiteSpaceCreatorUserId, userGroupName, null, userGroupDescription);

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("creatorUserId");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_NullOrWhiteSpaceName_ThrowsUserGroupException(string nullOrWhiteSpaceName)
        {
            var userGroupId = Guid.NewGuid();
            var creatorUserId = "User1";
            var memberIds = new List<string> { "User2" };
            var userGroupDescription = "GroupDescription";

            Func<UserGroup> sut = () => UserGroup.Create(userGroupId, creatorUserId, nullOrWhiteSpaceName, memberIds, userGroupDescription);

            sut.Should().Throw<UserGroupException>().WithMessage("*name*cannot be empty*");
        }

        [Fact]
        public void Edit_ValidData_StateChanges()
        {
            var userGroupId = Guid.NewGuid();
            var creatorUserId = "User1";
            var userGroupName = "GroupName";
            var memberIds = new List<string> { "User2" };
            var userGroupDescription = "GroupDescription";

            var sut = UserGroup.Create(userGroupId, creatorUserId, userGroupName, memberIds, userGroupDescription);

            var newName = "NewName";
            var newDescription = "NewDescription";

            sut.Edit(newName, newDescription);

            sut.Name.Should().Be(newName);
            sut.Description.Should().Be(newDescription);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Edit_NullOrWhiteSpaceName_ThrowsUserGroupException(string nullOrWhiteSpaceName)
        {
            var userGroupId = Guid.NewGuid();
            var creatorUserId = "User1";
            var userGroupName = "GroupName";
            var memberIds = new List<string> { "User2" };
            var userGroupDescription = "GroupDescription";

            var sut = UserGroup.Create(userGroupId, creatorUserId, userGroupName, memberIds, userGroupDescription);
            
            var newDescription = "NewDescription";

            sut.Invoking(s => s.Edit(nullOrWhiteSpaceName, newDescription)).Should().Throw<UserGroupException>()
                .WithMessage("*name*cannot be empty*");
        }
    }
}
