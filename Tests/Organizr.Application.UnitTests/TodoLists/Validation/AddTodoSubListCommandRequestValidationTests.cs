using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using FluentValidation;
using Moq;
using Organizr.Application.Common.Behaviors;
using Organizr.Application.TodoLists.Commands.AddTodoItem;
using Organizr.Application.TodoLists.Commands.AddTodoSubList;
using Organizr.Application.UnitTests.Common;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Validation
{
    public class AddTodoSubListCommandRequestValidationTests : RequestValidationTestBase<AddTodoSubListCommand>
    {
        protected override object[] ValidatorParams => null;

        public AddTodoSubListCommandRequestValidationTests()
        {
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new AddTodoSubListCommand(Guid.NewGuid(), "Title", "Description");

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().NotThrow();
        }

        [Fact]
        public void Handle_DefaultListId_ThrowsValidationException()
        {
            var request = new AddTodoSubListCommand(Guid.Empty, "Title", "Description");

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(AddTodoSubListCommand.TodoListId));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Handle_NullOrWhiteSpaceTitle_ThrowsValidationException(string nullOrWhiteSpaceTitle)
        {
            var request = new AddTodoSubListCommand(Guid.NewGuid(), nullOrWhiteSpaceTitle, "Description");

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(AddTodoSubListCommand.Title));
        }
    }
}
