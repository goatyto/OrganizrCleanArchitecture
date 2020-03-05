using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Organizr.Application.Common.Exceptions;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.Common.ResourceEntities.Commands.RemoveContributor
{
    public class RemoveContributorCommand : IRequest
    {
        public Guid ResourceId { get; }
        public string ContributorId { get; }

        public RemoveContributorCommand(Guid resourceId, string contributorId)
        {
            ResourceId = resourceId;
            ContributorId = contributorId;
        }
    }

    public class RemoveContributorCommandHandler<TResource> : IRequestHandler<RemoveContributorCommand> where TResource: ResourceEntity
    {
        private readonly IResourceEntityRepository _resourceEntityRepository;

        public RemoveContributorCommandHandler(IResourceEntityRepository resourceEntityRepository)
        {
            _resourceEntityRepository = resourceEntityRepository;
        }

        public async Task<Unit> Handle(RemoveContributorCommand request, CancellationToken cancellationToken)
        {
            var resourceEntity = await _resourceEntityRepository.GetByIdAsync<TResource>(request.ResourceId, cancellationToken);

            if (resourceEntity == null)
                throw new ResourceNotFoundException(request.ResourceId);

            resourceEntity.RemoveContributor(request.ContributorId);

            _resourceEntityRepository.Update(resourceEntity);

            await _resourceEntityRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
