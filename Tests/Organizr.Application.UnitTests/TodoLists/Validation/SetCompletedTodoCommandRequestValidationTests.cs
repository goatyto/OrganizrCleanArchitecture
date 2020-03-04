using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using FluentValidation;
using Moq;
using Organizr.Application.Common.Behaviors;
using Organizr.Application.TodoLists.Commands.SetCompletedTodoItem;
using Organizr.Application.UnitTests.Common;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Validation
{
    public class SetCompletedTodoCommandRequestValidationTests : RequestValidationTestBase<SetCompletedTodoItemCommand>
    {
        protected override object[] ValidatorParams => null;

        public SetCompletedTodoCommandRequestValidationTests()
        {

        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new SetCompletedTodoItemCommand(Guid.NewGuid(), 1, true);

            Sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>(), RequestHandlerDelegateMock.Object))
                .Should().NotThrow();
        }

        [Fact]
        public void Handle_DefaultTodoListId_ThrowsValidationException()
        {
            var request = new SetCompletedTodoItemCommand(Guid.Empty, 1, true);

            Sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>(), RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(SetCompletedTodoItemCommand.TodoListId));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Handle_NegativeOrZeroTodoId_ThrowsValidationException(int negativeOrZeroTodoId)
        {
            var request = new SetCompletedTodoItemCommand(Guid.NewGuid(), negativeOrZeroTodoId, true);

            Sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>(), RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(SetCompletedTodoItemCommand.Id));
        }
    }
}
