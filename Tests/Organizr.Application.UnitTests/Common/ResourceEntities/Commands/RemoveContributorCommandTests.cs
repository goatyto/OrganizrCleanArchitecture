using System;
using System.Threading;
using FluentAssertions;
using Moq;
using Organizr.Application.Common.Exceptions;
using Organizr.Application.Common.ResourceEntities.Commands.RemoveContributor;
using Organizr.Domain.SharedKernel;
using Xunit;

namespace Organizr.Application.UnitTests.Common.ResourceEntities.Commands
{
    public class RemoveContributorCommandTests: ResourceEntityCommandTestBase
    {
        private readonly RemoveContributorCommandHandler<ResourceEntityStub> _sut;

        public RemoveContributorCommandTests()
        {
            _sut = new RemoveContributorCommandHandler<ResourceEntityStub>(ResourceEntityRepositoryMock.Object);
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new RemoveContributorCommand(ResourceId, "User2");

            _sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>())).Should().NotThrow();
        }

        [Fact]
        public void Handle_NonExistentResourceId_ThrowsNotFoundException()
        {
            var nonExistentResourceId = Guid.NewGuid();

            var request = new RemoveContributorCommand(nonExistentResourceId, "User2");

            _sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>())).Should()
                .Throw<ResourceNotFoundException>().And.Id.Should().Be(nonExistentResourceId);
        }
    }
}
