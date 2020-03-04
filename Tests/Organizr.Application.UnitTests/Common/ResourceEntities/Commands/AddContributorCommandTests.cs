using System;
using System.Threading;
using FluentAssertions;
using Moq;
using Organizr.Application.Common.Exceptions;
using Organizr.Application.Common.ResourceEntities.Commands.AddContributor;
using Xunit;

namespace Organizr.Application.UnitTests.Common.ResourceEntities.Commands
{
    public class AddContributorCommandTests: ResourceEntityCommandTestBase
    {
        private readonly AddContributorCommandHandler _sut;

        public AddContributorCommandTests()
        {
            _sut = new AddContributorCommandHandler(ResourceEntityRepositoryMock.Object);
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new AddContributorCommand(ResourceId, "User4");

            _sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>())).Should().NotThrow();
        }

        [Fact]
        public void Handle_NonExistentResourceId_ThrowsNotFoundException()
        {
            var nonExistentResourceId = Guid.NewGuid();

            var request = new AddContributorCommand(nonExistentResourceId, "User4");

            _sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>())).Should()
                .Throw<ResourceNotFoundException>().And.Id.Should().Be(nonExistentResourceId);
        }
    }
}
