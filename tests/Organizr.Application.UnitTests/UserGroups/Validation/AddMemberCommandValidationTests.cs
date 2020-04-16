using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using FluentValidation;
using Organizr.Application.Planning.UserGroups.AddMember;
using Organizr.Application.UnitTests.Common;
using Xunit;

namespace Organizr.Application.UnitTests.UserGroups.Validation
{
    public class AddMemberCommandValidationTests: RequestValidationTestBase<AddMemberCommand>
    {
        protected override object[] ValidatorParams => null;

        [Fact]
        public void Handle_ValidData_DoesNotThrow()
        {
            var userGroupId = Guid.NewGuid();
            var memberUserId = "User1";

            var request = new AddMemberCommand(userGroupId, memberUserId);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object)).Should()
                .NotThrow();
        }

        [Fact]
        public void Handle_DefaultUserGroupId_ThrowsValidationException()
        {
            var defaultUserGroupId = Guid.Empty;
            var memberUserId = "User1";

            var request = new AddMemberCommand(defaultUserGroupId, memberUserId);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object)).Should()
                .Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(AddMemberCommand.UserGroupId));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Handle_EmptyMemberUserId_ThrowsValidationException(string emptyMemberUserId)
        {
            var userGroupId = Guid.NewGuid();

            var request = new AddMemberCommand(userGroupId, emptyMemberUserId);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object)).Should()
                .Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(AddMemberCommand.MemberUserId));
        }
    }
}
