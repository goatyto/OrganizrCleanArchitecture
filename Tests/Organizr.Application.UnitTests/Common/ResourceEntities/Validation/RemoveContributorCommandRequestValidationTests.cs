using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using FluentValidation;
using Moq;
using Organizr.Application.Common.Behaviors;
using Organizr.Application.Common.ResourceEntities.Commands.RemoveContributor;
using Xunit;

namespace Organizr.Application.UnitTests.Common.ResourceEntities.Validation
{
    public class RemoveContributorCommandRequestValidationTests : RequestValidationTestBase<RemoveContributorCommand>
    {
        protected override object[] ValidatorParams => null;

        public RemoveContributorCommandRequestValidationTests()
        {
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new RemoveContributorCommand(Guid.NewGuid(), "User1");

            Sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>(), RequestHandlerDelegateMock.Object))
                .Should().NotThrow();
        }

        [Fact]
        public void Handle_DefaultResourceId_ThrowsValidationException()
        {
            var request = new RemoveContributorCommand(Guid.Empty, "User1");

            Sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>(), RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(RemoveContributorCommand.ResourceId));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Handle_NullOrEmptyContributorId_ThrowsValidationException(string nullOrEmptyContributorId)
        {
            var request = new RemoveContributorCommand(Guid.NewGuid(), nullOrEmptyContributorId);

            Sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>(), RequestHandlerDelegateMock.Object))
                .Should().Throw<ValidationException>().And.Errors.Should().ContainSingle(failure =>
                    failure.PropertyName == nameof(RemoveContributorCommand.ContributorId));
        }
    }
}
