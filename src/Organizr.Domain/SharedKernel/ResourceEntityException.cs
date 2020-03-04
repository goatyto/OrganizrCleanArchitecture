using System;
using System.Collections.Generic;
using System.Text;

namespace Organizr.Domain.SharedKernel
{
    public abstract class ResourceEntityException : Exception
    {
        public ResourceEntityException(string message, Exception innerException = null) : base(message, innerException)
        {

        }
    }

    public class ContributorAlreadyOwnerException : ResourceEntityException
    {
        public Guid ResourceId { get; private set; }
        public string ContributorId { get; private set; }

        public ContributorAlreadyOwnerException(Guid resourceId, string contributorId, Exception innerException = null)
            : base($"Contributor \"{contributorId}\" is already set as an owner for resource \"{resourceId}\".",
                innerException)
        {
            ResourceId = resourceId;
            ContributorId = contributorId;
        }
    }

    public class ContributorAlreadyExistsException : ResourceEntityException
    {
        public Guid ResourceId { get; private set; }
        public string ContributorId { get; private set; }

        public ContributorAlreadyExistsException(Guid resourceId, string contributorId, Exception innerException = null)
            : base($"Contributor \"{contributorId}\" already set for resource \"{resourceId}\".", innerException)
        {
            ResourceId = resourceId;
            ContributorId = contributorId;
        }
    }
}
