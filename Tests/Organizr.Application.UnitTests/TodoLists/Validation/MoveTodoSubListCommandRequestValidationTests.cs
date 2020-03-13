using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using Organizr.Application.Common.Behaviors;
using Organizr.Application.TodoLists.Commands.MoveTodoSubList;
using Organizr.Application.UnitTests.Common;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Validation
{
    public class MoveTodoSubListCommandRequestValidationTests : RequestValidationTestBase<MoveTodoSubListCommand>
    {
        protected override object[] ValidatorParams => null;

        public MoveTodoSubListCommandRequestValidationTests()
        {
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new MoveTodoSubListCommand(Guid.NewGuid(), 1, 1);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().NotThrow();
        }

        [Fact]
        public void Handle_DefaultTodoListId_ThrowsValidationException()
        {
            var request = new MoveTodoSubListCommand(Guid.Empty, 1, 1);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(MoveTodoSubListCommand.TodoListId));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Handle_NegativeOrZeroTodoId_ThrowsValidationException(int negativeOrZeroTodoId)
        {
            var request = new MoveTodoSubListCommand(Guid.NewGuid(), negativeOrZeroTodoId, 1);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should()
                .ContainSingle(failure => failure.PropertyName == nameof(MoveTodoSubListCommand.Id));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Handle_NegativeOrZeroOrdinal_ThrowsValidationException(int negativeOrZeroOrdinal)
        {
            var request = new MoveTodoSubListCommand(Guid.NewGuid(), 1, negativeOrZeroOrdinal);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should()
                .ContainSingle(failure => failure.PropertyName == nameof(MoveTodoSubListCommand.NewOrdinal));
        }
    }
}
