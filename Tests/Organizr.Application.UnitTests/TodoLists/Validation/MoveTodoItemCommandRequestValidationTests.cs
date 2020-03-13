using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using Organizr.Application.Common.Behaviors;
using Organizr.Application.TodoLists.Commands.MoveTodoItem;
using Organizr.Application.UnitTests.Common;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Validation
{
    public class MoveTodoItemCommandRequestValidationTests : RequestValidationTestBase<MoveTodoItemCommand>
    {
        protected override object[] ValidatorParams => null;

        public MoveTodoItemCommandRequestValidationTests()
        {
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new MoveTodoItemCommand(Guid.NewGuid(), 1, 1, 1);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().NotThrow();
        }

        [Fact]
        public void Handle_DefaultTodoListId_ThrowsValidationException()
        {
            var request = new MoveTodoItemCommand(Guid.Empty, 1, 1, 1);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(MoveTodoItemCommand.TodoListId));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Handle_NegativeOrZeroTodoId_ThrowsValidationException(int negativeOrZeroTodoId)
        {
            var request = new MoveTodoItemCommand(Guid.NewGuid(), negativeOrZeroTodoId, 1, 1);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should()
                .ContainSingle(failure => failure.PropertyName == nameof(MoveTodoItemCommand.Id));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Handle_NegativeOrZeroOrdinal_ThrowsValidationException(int negativeOrZeroOrdinal)
        {
            var request = new MoveTodoItemCommand(Guid.NewGuid(), 1, negativeOrZeroOrdinal, 1);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should()
                .ContainSingle(failure => failure.PropertyName == nameof(MoveTodoItemCommand.NewOrdinal));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Handle_NegativeOrZeroSubListId_ThrowsValidationException(int negativeOrZeroSubListId)
        {
            var request = new MoveTodoItemCommand(Guid.NewGuid(), 1, 1, negativeOrZeroSubListId);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should()
                .ContainSingle(failure => failure.PropertyName == nameof(MoveTodoItemCommand.SubListId));
        }
    }
}
