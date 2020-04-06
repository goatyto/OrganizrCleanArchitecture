using System;
using System.Linq;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.UserGroupAggregate
{
    public class UserGroupMemberTests
    {
        private readonly UserGroupFixture _fixture;

        public UserGroupMemberTests()
        {
            _fixture = new UserGroupFixture();
        }

        [Fact]
        public void AddMember_ValidData_MemberAdded()
        {
            var newMemberId = "User3";

            _fixture.Sut.AddMember(newMemberId);

            var addedMemberId = _fixture.Sut.Members.Last();

            addedMemberId.UserId.Should().Be(newMemberId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddMember_NullOrWhiteSpaceUserId_ThrowsArgumentException(string nullOrWhiteSpaceUserId)
        {
            _fixture.Sut.Invoking(s => s.AddMember(nullOrWhiteSpaceUserId)).Should().Throw<ArgumentException>().And
                .ParamName.Should().Be("userId");
        }

        [Fact]
        public void AddMember_ExistingMemberId_ThrowsInvalidOperationException()
        {
            _fixture.Sut.Invoking(s => s.AddMember(_fixture.ExistingUserGroupMemberId)).Should()
                .Throw<InvalidOperationException>().WithMessage($"*user*{_fixture.ExistingUserGroupMemberId}*already a member*");
        }

        [Fact]
        public void RemoveMember_ValidData_MemberRemoved()
        {
            var initialMembershipCount = _fixture.Sut.Members.Count;

            _fixture.Sut.RemoveMember(_fixture.ExistingUserGroupMemberId);

            _fixture.Sut.Members.Should().HaveCount(initialMembershipCount - 1).And
                .NotContain(membership => membership.UserId == _fixture.ExistingUserGroupMemberId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void RemoveMember_NullOrWhiteSpaceUserId_ThrowsArgumentException(string nullOrWhiteSpaceUserId)
        {
            _fixture.Sut.Invoking(s => s.RemoveMember(nullOrWhiteSpaceUserId)).Should().Throw<ArgumentException>().And
                .ParamName.Should().Be("userId");
        }

        [Fact]
        public void RemoveMember_CreatorMemberId_ThrowsUserGroupException()
        {
            _fixture.Sut.Invoking(s => s.RemoveMember(_fixture.UserGroupCreatorId)).Should()
                .Throw<UserGroupException>().WithMessage("*creator*cannot be removed*");
        }

        [Fact]
        public void RemoveMember_NonExistingMemberId_ThrowsInvalidOperationException()
        {
            var nonExistentMemberUserId = "User99";

            _fixture.Sut.Invoking(s => s.RemoveMember(nonExistentMemberUserId)).Should()
                .Throw<InvalidOperationException>().WithMessage($"*user*{nonExistentMemberUserId}*not a member*");
        }
    }
}
