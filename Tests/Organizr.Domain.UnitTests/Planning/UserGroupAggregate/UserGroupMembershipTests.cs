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
        private readonly UserGroupFixture _fixture;

        public UserGroupMembershipTests()
        {
            _fixture = new UserGroupFixture();
        }

        [Fact]
        public void AddMember_ValidData_MemberAdded()
        {
            var newMemberId = new UserGroupMember("User3");
            _fixture.Sut.AddMember(newMemberId);

            var addedMemberId = _fixture.Sut.Members.Last();

            addedMemberId.Should().Be(newMemberId);
        }

        [Fact]
        public void AddMember_ExistingMemberId_ThrowsUserAlreadyMemberException()
        {
            _fixture.Sut.Invoking(s => s.AddMember(_fixture.ExistingUserGroupMember)).Should()
                .Throw<UserGroupException>().WithMessage($"*user*{_fixture.ExistingUserGroupMember}*already a member*");
        }

        [Fact]
        public void RemoveMember_ValidData_MemberRemoved()
        {
            var initialMembershipCount = _fixture.Sut.Members.Count;

            _fixture.Sut.RemoveMember(_fixture.ExistingUserGroupMember);

            _fixture.Sut.Members.Should().HaveCount(initialMembershipCount - 1).And
                .NotContain(member => member == _fixture.ExistingUserGroupMember);
        }

        [Fact]
        public void RemoveMember_CreatorMemberId_ThrowsCreatorCannotBeRemovedException()
        {
            _fixture.Sut.Invoking(s => s.RemoveMember((UserGroupMember)_fixture.UserGroupCreator)).Should()
                .Throw<UserGroupException>().WithMessage("*creator*cannot be removed*");
        }

        [Fact]
        public void RemoveMember_NonExistingMemberId_ThrowsUserNotAMemberException()
        {
            var nonExistentMember = new UserGroupMember("User99");

            _fixture.Sut.Invoking(s => s.RemoveMember(nonExistentMember)).Should()
                .Throw<UserGroupException>().WithMessage($"*user*{nonExistentMember}*not a member*");
        }
    }
}
