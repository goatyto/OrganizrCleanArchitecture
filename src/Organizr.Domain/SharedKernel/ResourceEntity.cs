using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;

namespace Organizr.Domain.SharedKernel
{
    public abstract class ResourceEntity : Entity<Guid>
    {
        public virtual string OwnerId { get; protected set; }

        protected readonly List<string> _contributorIds;
        public IReadOnlyCollection<string> ContributorIds => _contributorIds.AsReadOnly();

        protected ResourceEntity()
        {
            _contributorIds = new List<string>();
        }

        public void AddContributor(string contributorId)
        {
            Guard.Against.NullOrWhiteSpace(contributorId, nameof(contributorId));

            if (OwnerId == contributorId)
                throw new ContributorAlreadyOwnerException(Id, contributorId);

            if (ContributorIds.Any(id => id == contributorId))
                throw new ContributorAlreadyExistsException(Id, contributorId);

            _contributorIds.Add(contributorId);
        }

        public void RemoveContributor(string contributorId)
        {
            Guard.Against.NullOrWhiteSpace(contributorId, nameof(contributorId));

            if (ContributorIds.All(id => id != contributorId))
                throw new ContributorDoesNotExistException(Id, contributorId);

            _contributorIds.Remove(contributorId);
        }
    }
}
