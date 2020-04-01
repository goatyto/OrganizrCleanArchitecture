using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Organizr.Domain.Planning;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.UserGroupAggregate
{
    public class UserGroupTests
    {
        [Fact]
        public void Create_ValidData_ObjectInitialized()
        {
            var id = new UserGroupId(Guid.NewGuid());
            var name = "GroupName";
            var creatorUser = new CreatorUser("User1");
            var description = "GroupDescription";
            var memberUserIds = new List<UserGroupMember>
            {
                new UserGroupMember("User1"),
                new UserGroupMember("User2"),
                new UserGroupMember("User3")
            };

            var sut = UserGroup.Create(id, creatorUser, name, description, memberUserIds);

            sut.UserGroupId.Should().Be(id);
            sut.Name.Should().Be(name);
            sut.CreatorUser.Should().Be(creatorUser);
            sut.Description.Should().Be(description);
            sut.Members.Should().NotBeNull().And.Contain(memberUserIds);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_NullOrWhiteSpaceName_ThrowsArgumentException(string nullOrWhiteSpaceName)
        {
            Func<UserGroup> sut = () =>
                UserGroup.Create(new UserGroupId(Guid.NewGuid()), new CreatorUser("User1"), nullOrWhiteSpaceName, "GroupDescription");

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("name");
        }

        [Fact]
        public void Edit_ValidData_StateChanges()
        {
            var newName = "NewName";
            var newDescription = "NewDescription";

            var sut = UserGroup.Create(new UserGroupId(Guid.NewGuid()), new CreatorUser("User1"), "GroupName", "GroupDescription");

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

            var sut = UserGroup.Create(new UserGroupId(Guid.NewGuid()), new CreatorUser("User1"), "GroupName", "GroupDescription");

            sut.Invoking(s => s.Edit(nullOrWhiteSpaceName, newDescription)).Should().Throw<ArgumentException>().And
                .ParamName.Should().Be("name");
        }
    }
}
