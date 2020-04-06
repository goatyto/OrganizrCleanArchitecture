using System;
using System.Threading;
using FluentAssertions;
using FluentValidation;
using Organizr.Application.Planning.TodoLists.Commands.DeleteTodoSubList;
using Organizr.Application.UnitTests.Common;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Validation
{
    public class DeleteTodoSubListCommandRequestValidationTests : RequestValidationTestBase<DeleteTodoSubListCommand>
    {
        protected override object[] ValidatorParams => null;

        public DeleteTodoSubListCommandRequestValidationTests()
        {

        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new DeleteTodoSubListCommand(Guid.NewGuid(), 1);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().NotThrow();
        }

        [Fact]
        public void Handle_DefaultTodoListId_ThrowsValidationException()
        {
            var request = new DeleteTodoSubListCommand(Guid.Empty, 1);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(DeleteTodoSubListCommand.TodoListId));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Handle_NegativeOrZeroTodoSubListId_ThrowsValidationException(int negativeOrZeroTodoSubListId)
        {
            var request = new DeleteTodoSubListCommand(Guid.NewGuid(), negativeOrZeroTodoSubListId);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(DeleteTodoSubListCommand.Id));
        }
    }
}
