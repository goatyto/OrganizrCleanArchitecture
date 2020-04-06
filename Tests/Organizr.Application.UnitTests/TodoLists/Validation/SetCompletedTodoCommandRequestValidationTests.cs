using System;
using System.Threading;
using FluentAssertions;
using FluentValidation;
using Organizr.Application.Planning.TodoLists.Commands.SetCompletedTodoItem;
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

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().NotThrow();
        }

        [Fact]
        public void Handle_DefaultTodoListId_ThrowsValidationException()
        {
            var request = new SetCompletedTodoItemCommand(Guid.Empty, 1, true);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(SetCompletedTodoItemCommand.TodoListId));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Handle_NegativeOrZeroTodoId_ThrowsValidationException(int negativeOrZeroTodoId)
        {
            var request = new SetCompletedTodoItemCommand(Guid.NewGuid(), negativeOrZeroTodoId, true);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(SetCompletedTodoItemCommand.Id));
        }
    }
}
