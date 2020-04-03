using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.UserGroupAggregate
{
    public class UserGroupMembershipTests
    {
        [Fact]
        public void AddMember_ValidData_MemberAdded()
        {
            var fixture = new UserGroupFixture();

            var newMemberId = "User3";
            fixture.Sut.AddMember(newMemberId);

            var addedMemberId = fixture.Sut.Members.Last();

            addedMemberId.UserId.Should().Be(newMemberId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddMember_NullOrWhiteSpaceUserId_ThrowsArgumentException(string nullOrWhiteSpaceUserId)
        {
            var fixture = new UserGroupFixture();

            fixture.Sut.Invoking(s => s.AddMember(nullOrWhiteSpaceUserId)).Should().Throw<ArgumentException>().And
                .ParamName.Should().Be("userId");
        }

        [Fact]
        public void AddMember_ExistingMemberId_ThrowsUserAlreadyMemberException()
        {
            var fixture = new UserGroupFixture();

            fixture.Sut.Invoking(s => s.AddMember(fixture.ExistingUserGroupMemberId)).Should()
                .Throw<UserAlreadyMemberException>().Where(exception =>
                    exception.UserGroupId == fixture.UserGroupId &&
                    exception.UserId == fixture.ExistingUserGroupMemberId);
        }

        [Fact]
        public void RemoveMember_ValidData_MemberRemoved()
        {
            var fixture = new UserGroupFixture();
            var initialMembershipCount = fixture.Sut.Members.Count;

            fixture.Sut.RemoveMember(fixture.ExistingUserGroupMemberId);

            fixture.Sut.Members.Should().HaveCount(initialMembershipCount - 1).And
                .NotContain(membership => membership.UserId == fixture.ExistingUserGroupMemberId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void RemoveMember_NullOrWhiteSpaceUserId_ThrowsArgumentException(string nullOrWhiteSpaceUserId)
        {
            var fixture = new UserGroupFixture();

            fixture.Sut.Invoking(s => s.RemoveMember(nullOrWhiteSpaceUserId)).Should().Throw<ArgumentException>().And
                .ParamName.Should().Be("userId");
        }

        [Fact]
        public void RemoveMember_CreatorMemberId_ThrowsCreatorCannotBeRemovedException()
        {
            var fixture = new UserGroupFixture();

            fixture.Sut.Invoking(s => s.RemoveMember(fixture.UserGroupCreatorId)).Should()
                .Throw<CreatorCannotBeRemovedException>()
                .Where(exception => exception.UserGroupId == fixture.UserGroupId);
        }

        [Fact]
        public void RemoveMember_NonExistingMemberId_ThrowsUserNotAMemberException()
        {
            var fixture = new UserGroupFixture();

            var nonExistentMemberUserId = "User99";

            fixture.Sut.Invoking(s => s.RemoveMember(nonExistentMemberUserId)).Should()
                .Throw<UserNotAMemberException>().Where(exception =>
                    exception.UserGroupId == fixture.UserGroupId &&
                    exception.UserId == nonExistentMemberUserId);
        }
    }
}
