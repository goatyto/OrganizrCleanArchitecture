using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using Organizr.Domain.Guards;

namespace Organizr.Domain.SharedKernel
{
    public abstract class ResourceEntity : Entity<Guid>, IAggregateRoot
    {
        public virtual string OwnerId { get; protected set; }

        protected readonly List<ResourceContributor> _resourceContributors;
        public IReadOnlyCollection<ResourceContributor> ResourceContributors => _resourceContributors.AsReadOnly();

        protected ResourceEntity()
        {
            _resourceContributors = new List<ResourceContributor>();
        }

        public void AddContributor(string contributorId)
        {
            Guard.Against.NullOrWhiteSpace(contributorId, nameof(contributorId));

            if (OwnerId == contributorId)
                throw new ContributorAlreadyOwnerException(Id, contributorId);

            var resourceContributor = new ResourceContributor(Id, contributorId);

            if (ResourceContributors.Any(rc=>rc == resourceContributor))
                throw new ContributorAlreadyExistsException(Id, contributorId);

            _resourceContributors.Add(resourceContributor);
        }

        public void RemoveContributor(string contributorId)
        {
            Guard.Against.NullOrWhiteSpace(contributorId, nameof(contributorId));

            var resourceContributor = ResourceContributors.SingleOrDefault(rc => rc.ContributorId == contributorId);

            Guard.Against.NullQueryResult(resourceContributor, nameof(contributorId));

            _resourceContributors.Remove(resourceContributor);
        }
    }
}
