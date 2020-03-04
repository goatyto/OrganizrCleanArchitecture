using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using Organizr.Application.Common.Behaviors;
using Organizr.Application.TodoLists.Commands.EditTodoList;
using Organizr.Application.UnitTests.Common;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Validation
{
    public class EditTodoListCommandRequestValidationTests : RequestValidationTestBase<EditTodoListCommand>
    {
        protected override object[] ValidatorParams => null;

        public EditTodoListCommandRequestValidationTests()
        {
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new EditTodoListCommand(Guid.NewGuid(),"Title", "Description");

            Sut.Invoking(s=>s.Handle(request, It.IsAny<CancellationToken>(), RequestHandlerDelegateMock.Object)).Should().NotThrow();
        }

        [Fact]
        public void Handle_DefaultTodoListId_ThrowsValidationException()
        {
            var request = new EditTodoListCommand(Guid.Empty,"Title", "Description");

            Sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>(), RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(EditTodoListCommand.Id));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Handle_NullOrWhiteSpaceTitle_ThrowsValidationException(string nullOrWhiteSpaceTitle)
        {
            var request = new EditTodoListCommand(Guid.NewGuid(),nullOrWhiteSpaceTitle, "Description");

            Sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>(), RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(EditTodoListCommand.Title));
        }
    }
}
