using System;
using System.Threading;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using Organizr.Application.Common.Behaviors;
using Organizr.Application.TodoLists.Commands.AddTodoItem;
using Organizr.Application.UnitTests.Common;
using Organizr.Domain.Planning.Services;
using Organizr.Domain.SharedKernel;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Validation
{
    public class AddTodoItemCommandRequestValidationTests : RequestValidationTestBase<AddTodoItemCommand>
    {
        protected override object[] ValidatorParams => new object[] { new ClientDateValidator() };
        private readonly DateTime ClientDateToday = DateTime.UtcNow.Date;
        private readonly int _clientTimeZoneOffsetInMinutes = 0;

        public AddTodoItemCommandRequestValidationTests()
        {

        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new AddTodoItemCommand(Guid.NewGuid(), "Title", "Description", ClientDateToday.AddDays(1), _clientTimeZoneOffsetInMinutes, 1);

            Sut.Invoking(s =>
                    s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object)).Should()
                .NotThrow();
        }

        [Fact]
        public void Handle_DefaultListId_ThrowsValidationException()
        {
            var request = new AddTodoItemCommand(Guid.Empty, "Title", "Description", ClientDateToday.AddDays(1), _clientTimeZoneOffsetInMinutes, 1);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object)).Should()
                .Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(AddTodoItemCommand.TodoListId));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Handle_NullOrEmptyTitle_ThrowsValidationException(string nullOrEmptyTitle)
        {
            var request = new AddTodoItemCommand(Guid.NewGuid(), nullOrEmptyTitle, "Description", ClientDateToday.AddDays(1), _clientTimeZoneOffsetInMinutes, 1);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(AddTodoItemCommand.Title));
        }

        [Fact]
        public void Handle_DueDateInThePast_ThrowsValidationException()
        {
            var request = new AddTodoItemCommand(Guid.NewGuid(), "Title", "Description", ClientDateToday.AddDays(-1),
                _clientTimeZoneOffsetInMinutes, 1);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(AddTodoItemCommand.DueDateUtc));
        }

        [Fact]
        public void Handle_DueDateWithTimeComponent_ThrowsValidationException()
        {
            var request = new AddTodoItemCommand(Guid.NewGuid(), "Title", "Description", ClientDateToday.AddTicks(1),
                _clientTimeZoneOffsetInMinutes, 1);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(AddTodoItemCommand.DueDateUtc));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Handle_NegativeOrNullSubListId_ThrowsValidationException(int negativeOrNullSubListid)
        {
            var request = new AddTodoItemCommand(Guid.NewGuid(), "Title", "Description", ClientDateToday.AddDays(1),
                _clientTimeZoneOffsetInMinutes, negativeOrNullSubListid);

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(AddTodoItemCommand.SubListId));
        }
    }
}
