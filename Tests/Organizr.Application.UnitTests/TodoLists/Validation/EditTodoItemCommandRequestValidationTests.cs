using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using Organizr.Application.Common.Behaviors;
using Organizr.Application.TodoLists.Commands.EditTodoItem;
using Organizr.Application.TodoLists.Commands.EditTodoList;
using Organizr.Application.UnitTests.Common;
using Organizr.Domain.Planning.Services;
using Organizr.Domain.SharedKernel;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Validation
{
    public class EditTodoItemCommandRequestValidationTests : RequestValidationTestBase<EditTodoItemCommand>
    { 
        protected override object[] ValidatorParams => new object[] { new ClientDateValidator() };
        private readonly DateTime ClientDateToday = DateTime.UtcNow.Date;
        private readonly int _clientTimeZoneOffsetInMinutes = 0;

        public EditTodoItemCommandRequestValidationTests()
        {

        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new EditTodoItemCommand(Guid.NewGuid(), 1, "Title", "Description",
                ClientDateToday.AddDays(1), _clientTimeZoneOffsetInMinutes);

            Sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>(), RequestHandlerDelegateMock.Object)).Should().NotThrow();
        }

        [Fact]
        public void Handle_DefaultTodoListId_ThrowsValidationException()
        {
            var request = new EditTodoItemCommand(Guid.Empty, 1, "Title", "Description",
                ClientDateToday.AddDays(1), _clientTimeZoneOffsetInMinutes);

            Sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>(), RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(EditTodoItemCommand.TodoListId));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Handle_NegativeOrZeroTodoId_ThrowsValidationException(int negativeOrZeroTodoId)
        {
            var request = new EditTodoItemCommand(Guid.NewGuid(), negativeOrZeroTodoId, "Title", "Description",
                ClientDateToday.AddDays(1), _clientTimeZoneOffsetInMinutes);

            Sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>(), RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should()
                .ContainSingle(failure => failure.PropertyName == nameof(EditTodoItemCommand.Id));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Handle_NullOrWhiteSpaceTitle_ThrowsValidationException(string nullOrWhiteSpaceTitle)
        {
            var request = new EditTodoItemCommand(Guid.NewGuid(), 1, nullOrWhiteSpaceTitle, "Description",
                ClientDateToday.AddDays(1), _clientTimeZoneOffsetInMinutes);

            Sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>(), RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(EditTodoItemCommand.Title));
        }

        [Fact]
        public void Handle_DueDateInThePast_ThrowsValidationException()
        {
            var request = new EditTodoItemCommand(Guid.NewGuid(), 1, "Title", "Description",
                ClientDateToday.AddDays(-1), _clientTimeZoneOffsetInMinutes);

            Sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>(), RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(EditTodoItemCommand.DueDateUtc));
        }
    }
}
