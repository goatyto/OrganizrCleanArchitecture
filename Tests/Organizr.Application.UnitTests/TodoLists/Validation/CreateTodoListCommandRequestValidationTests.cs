using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using Organizr.Application.Common.Behaviors;
using Organizr.Application.TodoLists.Commands.CreateTodoList;
using Organizr.Application.UnitTests.Common;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Validation
{
    public class CreateTodoListCommandRequestValidationTests : RequestValidationTestBase<CreateTodoListCommand>
    {
        protected override object[] ValidatorParams => null;

        public CreateTodoListCommandRequestValidationTests()
        {
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new CreateTodoListCommand("Title", "Description");

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().NotThrow();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Handle_NullOrWhiteSpaceTitle_ThrowsValidationException(string nullOrWhiteSpaceTitle)
        {
            var request = new CreateTodoListCommand(nullOrWhiteSpaceTitle, "Description");

            Sut.Invoking(s => s.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(CreateTodoListCommand.Title));
        }
    }
}
