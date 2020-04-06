using System;
using System.Threading;
using FluentAssertions;
using FluentValidation;
using Organizr.Application.Planning.TodoLists.Commands.DeleteTodoItem;
using Organizr.Application.UnitTests.Common;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Validation
{
    public class DeleteTodoItemCommandRequestValidationTests : RequestValidationTestBase<DeleteTodoItemCommand>
    {
        protected override object[] ValidatorParams => null;

        public DeleteTodoItemCommandRequestValidationTests()
        {
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new DeleteTodoItemCommand(Guid.NewGuid(), 1);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().NotThrow();
        }

        [Fact]
        public void Handle_DefaultTodoListId_ThrowsValidationException()
        {
            var request = new DeleteTodoItemCommand(Guid.Empty, 1);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(DeleteTodoItemCommand.TodoListId));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Handle_NegativeOrZeroTodoId_ThrowsValidationException(int negativeOrZeroTodoId)
        {
            var request = new DeleteTodoItemCommand(Guid.NewGuid(), negativeOrZeroTodoId);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(DeleteTodoItemCommand.Id));
        }
    }
}
