using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.UnitTests.Common.ResourceEntities.Commands
{
    public abstract class ResourceEntityCommandTestBase
    {
        protected readonly Guid ResourceId;
        protected readonly Mock<IResourceEntityRepository> ResourceEntityRepositoryMock;

        public ResourceEntityCommandTestBase()
        {
            ResourceId = Guid.NewGuid();

            ResourceEntityRepositoryMock = new Mock<IResourceEntityRepository>();
            ResourceEntityRepositoryMock
                .Setup(repository => repository.GetByIdAsync<ResourceEntityStub>(ResourceId, It.IsAny<CancellationToken>())).ReturnsAsync(
                    new ResourceEntityStub("User1", new List<ResourceContributor>
                    {
                        new ResourceContributor(ResourceId, "User2")
                    }));
            ResourceEntityRepositoryMock
                .Setup(repository => repository.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }
    }

    public class ResourceEntityStub : ResourceEntity, IAggregateRoot
    {
        public ResourceEntityStub(string ownerId, IEnumerable<ResourceContributor> contributors = null)
        {
            OwnerId = ownerId;

            if(contributors != null)
                foreach(var contributor in contributors)
                    _resourceContributors.Add(contributor);
        }
    }
}
