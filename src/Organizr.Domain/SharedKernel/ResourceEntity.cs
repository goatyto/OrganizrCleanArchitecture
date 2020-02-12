using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;

namespace Organizr.Domain.SharedKernel
{
    public abstract class ResourceEntity : Entity<Guid>
    {
        private string _ownerId;

        public virtual string OwnerId
        {
            get => _ownerId;
            protected set
            {
                Guard.Against.NullOrWhiteSpace(value, nameof(OwnerId));
                _ownerId = value;
            }
        }

        protected List<string> _contributorIds;
        public IReadOnlyCollection<string> ContributorIds => _contributorIds.AsReadOnly();

        protected ResourceEntity() : base()
        {

        }

        protected ResourceEntity(Guid id, string ownerId) : base(id)
        {
            OwnerId = ownerId;

            _contributorIds = new List<string>();
        }

        public void AddContributor(string contributorId)
        {
            if (OwnerId == contributorId)
                throw new OwnerSetAsContributorException();

            if (ContributorIds.Any(id => id == contributorId))
                throw new ContributorAlreadyExistsException(Id, contributorId);

            _contributorIds.Add(contributorId);
        }

        public void RemoveContributor(string contributorId)
        {
            if (ContributorIds.All(id => id != contributorId))
                throw new ContributorDoesNotExistException(Id, contributorId);

            _contributorIds.Remove(contributorId);
        }
    }
}
