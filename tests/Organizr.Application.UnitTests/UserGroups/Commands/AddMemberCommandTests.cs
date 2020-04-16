using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using Organizr.Application.Planning.Common.Exceptions;
using Organizr.Application.Planning.UserGroups.AddMember;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Organizr.Domain.SharedKernel;
using Xunit;

namespace Organizr.Application.UnitTests.UserGroups.Commands
{
    public class AddMemberCommandTests : UserGroupCommandTestsBase
    {
        private readonly AddMemberCommandHandler _sut;

        public AddMemberCommandTests()
        {
            _sut = new AddMemberCommandHandler(IdentityServiceMock.Object, UserGroupRepositoryMock.Object);
        }

        [Fact]
        public void Handle_ValidUserId_DoesNotThrow()
        {
            var request = new AddMemberCommand(UserGroupId, ValidNewMemberUserId);

            _sut.Invoking(s=>s.Handle(request, CancellationToken.None)).Should().NotThrow();
        }

        [Fact]
        public void Handle_NonExistentUserGroupId_ThrowsResourceNotFoundException()
        {
            var nonExistentUserGroupId = Guid.NewGuid();

            var request = new AddMemberCommand(nonExistentUserGroupId, ValidNewMemberUserId);

            _sut.Invoking(s => s.Handle(request, CancellationToken.None)).Should()
                .Throw<ResourceNotFoundException<UserGroup>>().And.ResourceId.Should().Be(nonExistentUserGroupId);
        }

        [Fact]
        public void Handle_NonExistentUserId_ThrowsInvalidUserIdException()
        {
            var nonExistentUserId = "User4";
            var request = new AddMemberCommand(UserGroupId, nonExistentUserId);

            _sut.Invoking(s => s.Handle(request, CancellationToken.None)).Should().Throw<InvalidUserIdException>().And
                .UserId.Should().Be(nonExistentUserId);
        }
    }
}
