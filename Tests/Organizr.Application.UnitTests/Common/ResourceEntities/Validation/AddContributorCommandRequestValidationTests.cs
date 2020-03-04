using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using FluentValidation;
using Moq;
using Organizr.Application.Common.Behaviors;
using Organizr.Application.Common.ResourceEntities.Commands.AddContributor;
using Xunit;

namespace Organizr.Application.UnitTests.Common.ResourceEntities.Validation
{
    public class AddContributorCommandRequestValidationTests : RequestValidationTestBase<AddContributorCommand>
    {
        protected override object[] ValidatorParams => null;

        public AddContributorCommandRequestValidationTests()
        {
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new AddContributorCommand(Guid.NewGuid(), "User1");

            Sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>(), RequestHandlerDelegateMock.Object))
                .Should().NotThrow();
        }

        [Fact]
        public void Handle_DefaultResourceId_ThrowsValidationException()
        {
            var request = new AddContributorCommand(Guid.Empty, "User1");

            Sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>(), RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(AddContributorCommand.ResourceId));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Handle_NullOrEmptyContributorId_ThrowsValidationException(string nullOrEmptyContributorId)
        {
            var request = new AddContributorCommand(Guid.NewGuid(), nullOrEmptyContributorId);

            Sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>(), RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(AddContributorCommand.ContributorId));
        }
    }
}
