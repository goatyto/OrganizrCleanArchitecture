using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using FluentValidation;
using Organizr.Application.Planning.UserGroups.CreateSharedTodoList;
using Organizr.Application.UnitTests.Common;
using Xunit;

namespace Organizr.Application.UnitTests.UserGroups.Validation
{
    public class CreateSharedTodoListCommandValidationTests : RequestValidationTestBase<CreateSharedTodoListCommand>
    {
        protected override object[] ValidatorParams => null;

        [Fact]
        public void Handle_ValidData_DoesNotThrow()
        {
            var userGroupId = Guid.NewGuid();
            var todoListTitle = "Title";

            var request = new CreateSharedTodoListCommand(userGroupId, todoListTitle);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object)).Should()
                .NotThrow();
        }

        [Fact]
        public void Handle_DefaultUserGroupId_ThrowsValidationException()
        {
            var defaultUserGroupId = Guid.Empty;
            var todoListTitle = "Title";

            var request = new CreateSharedTodoListCommand(defaultUserGroupId, todoListTitle);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object)).Should()
                .Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(CreateSharedTodoListCommand.UserGroupId));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Handle_EmptyTodoListTitle_ThrowsValidationException(string emptyTodoListTitle)
        {
            var userGroupId = Guid.NewGuid();

            var request = new CreateSharedTodoListCommand(userGroupId, emptyTodoListTitle);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object)).Should()
                .Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(CreateSharedTodoListCommand.Title));
        }
    }
}
