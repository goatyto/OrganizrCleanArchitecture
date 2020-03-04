using System;
using System.Threading;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using Organizr.Application.Common.Behaviors;
using Organizr.Application.TodoLists.Commands.AddTodoItem;
using Organizr.Application.UnitTests.Common;
using Organizr.Domain.SharedKernel;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Validation
{
    public class AddTodoItemCommandRequestValidationTests : RequestValidationTestBase<AddTodoItemCommand>
    {
        private readonly Mock<IDateTime> _dateTimeProviderMock;

        protected override object[] ValidatorParams => new object[] { _dateTimeProviderMock.Object };

        public AddTodoItemCommandRequestValidationTests()
        {
            _dateTimeProviderMock = new Mock<IDateTime>();
            _dateTimeProviderMock.Setup(m => m.Now).Returns(DateTime.UtcNow);
            _dateTimeProviderMock.Setup(m => m.Today).Returns(DateTime.UtcNow.Date);
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new AddTodoItemCommand(Guid.NewGuid(), "Title", "Description", _dateTimeProviderMock.Object.Today.AddDays(1), 1);

            Sut.Invoking(s =>
                    s.Handle(request, It.IsAny<CancellationToken>(), RequestHandlerDelegateMock.Object)).Should()
                .NotThrow();
        }

        [Fact]
        public void Handle_DefaultListId_ThrowsValidationException()
        {
            var request = new AddTodoItemCommand(Guid.Empty, "Title", "Description", _dateTimeProviderMock.Object.Today.AddDays(1), 1);

            Sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>(), RequestHandlerDelegateMock.Object)).Should()
                .Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(AddTodoItemCommand.TodoListId));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Handle_NullOrEmptyTitle_ThrowsValidationException(string nullOrEmptyTitle)
        {
            var request = new AddTodoItemCommand(Guid.NewGuid(), nullOrEmptyTitle, "Description", _dateTimeProviderMock.Object.Today.AddDays(1), 1);

            Sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>(), RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(AddTodoItemCommand.Title));
        }

        [Fact]
        public void Handle_DueDateInThePast_ThrowsValidationException()
        {
            var request = new AddTodoItemCommand(Guid.NewGuid(), "Title", "Description", _dateTimeProviderMock.Object.Today.AddDays(-1), 1);

            Sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>(), RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(AddTodoItemCommand.DueDate));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Handle_NonExistentSubListId_ThrowsValidationException(int invalidSubListId)
        {
            var request = new AddTodoItemCommand(Guid.NewGuid(), "Title", "Description", _dateTimeProviderMock.Object.Today.AddDays(1), invalidSubListId);

            Sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>(), RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(AddTodoItemCommand.SubListId));
        }
    }
}
