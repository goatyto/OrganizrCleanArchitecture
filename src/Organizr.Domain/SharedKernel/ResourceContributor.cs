using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;

namespace Organizr.Domain.SharedKernel
{
    public class ResourceContributor : ValueObject
    {
        public Guid ResourceId { get; }
        public string ContributorId { get; }

        public ResourceContributor(Guid resourceId, string contributorId)
        {
            Guard.Against.NullOrWhiteSpace(contributorId, nameof(contributorId));

            ResourceId = resourceId;
            ContributorId = contributorId;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ResourceId;
            yield return ContributorId;
        }
    }
}