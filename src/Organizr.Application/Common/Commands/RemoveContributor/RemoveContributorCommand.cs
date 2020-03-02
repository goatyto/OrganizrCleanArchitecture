using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.Common.Commands.RemoveContributor
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

    public class RemoveContributorCommandHandler : IRequestHandler<RemoveContributorCommand>
    {
        private readonly IResourceEntityRepository _resourceEntityRepository;

        public RemoveContributorCommandHandler(IResourceEntityRepository resourceEntityRepository)
        {
            _resourceEntityRepository = resourceEntityRepository;
        }

        public async Task<Unit> Handle(RemoveContributorCommand request, CancellationToken cancellationToken)
        {
            var resourceEntity = await _resourceEntityRepository.GetByIdAsync(request.ResourceId, cancellationToken);

            resourceEntity.RemoveContributor(request.ContributorId);

            _resourceEntityRepository.Update(resourceEntity);

            await _resourceEntityRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
