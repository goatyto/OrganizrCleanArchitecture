using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using Organizr.Application.Planning.TodoLists.Commands.EditTodoSubList;
using Organizr.Application.UnitTests.Common;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Validation
{
    public class EditTodoSubListCommandRequestValidationTests : RequestValidationTestBase<EditTodoSubListCommand>
    {
        protected override object[] ValidatorParams => null;

        public EditTodoSubListCommandRequestValidationTests()
        {
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new EditTodoSubListCommand(Guid.NewGuid(), 1, "Title", "Description");

            Sut.Invoking(s=>s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object)).Should().NotThrow();
        }

        [Fact]
        public void Handle_DefaultTodoListId_ThrowsValidationException()
        {
            var request = new EditTodoSubListCommand(Guid.Empty, 1, "Title", "Description");

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(EditTodoSubListCommand.TodoListId));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Handle_NegativeOrZeroSubListId_ThrowsValidationException(int negativeOrZeroSubListId)
        {
            var request = new EditTodoSubListCommand(Guid.NewGuid(), negativeOrZeroSubListId, "Title", "Description");

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should()
                .ContainSingle(failure => failure.PropertyName == nameof(EditTodoSubListCommand.Id));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Handle_NullOrWhiteSpaceTitle_ThrowsValidationException(string nullOrWhiteSpaceTitle)
        {
            var request = new EditTodoSubListCommand(Guid.NewGuid(), 1, nullOrWhiteSpaceTitle, "Description");

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(EditTodoSubListCommand.Title));
        }
    }
}
