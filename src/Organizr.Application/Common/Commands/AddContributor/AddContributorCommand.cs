using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.Common.Commands.AddContributor
{
    public class AddContributorCommand : IRequest
    {
        public Guid ResourceId { get; }
        public string ContributorId { get; }

        public AddContributorCommand(Guid resourceId, string contributorId)
        {
            ResourceId = resourceId;
            ContributorId = contributorId;
        }
    }

    public class AddContributorCommandHandler : IRequestHandler<AddContributorCommand>
    {
        private readonly IResourceEntityRepository _resourceEntityRepository;

        public AddContributorCommandHandler(IResourceEntityRepository resourceEntityRepository)
        {
            _resourceEntityRepository = resourceEntityRepository;
        }

        public async Task<Unit> Handle(AddContributorCommand request, CancellationToken cancellationToken)
        {
            var resourceEntity = await _resourceEntityRepository.GetByIdAsync(request.ResourceId, cancellationToken);

            resourceEntity.AddContributor(request.ContributorId);

            _resourceEntityRepository.Update(resourceEntity);

            await _resourceEntityRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
