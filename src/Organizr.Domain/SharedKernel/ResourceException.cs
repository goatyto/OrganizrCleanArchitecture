using System;
using System.Collections.Generic;
using System.Text;

namespace Organizr.Domain.SharedKernel
{
    public abstract class ResourceException : Exception
    {
        public ResourceException(string message, Exception innerException = null) : base(message, innerException)
        {

        }
    }

    public class OwnerSetAsContributorException : ResourceException
    {
        public OwnerSetAsContributorException(Exception innerException = null) : base("Cannot add owner as contributor", innerException)
        {
        }
    }

    public class ContributorAlreadyExistsException : ResourceException
    {
        public ContributorAlreadyExistsException(Guid resourceId, string contributorId, Exception innerException = null)
            : base($"Contributor \"{contributorId}\" already set for resource \"{resourceId}\"", innerException)
        {
        }
    }

    public class ContributorDoesNotExistException : ResourceException
    {
        public ContributorDoesNotExistException(Guid resourceId, string contributorId, Exception innerException = null)
            : base($"Contributor \"{contributorId}\" does not exist for resource \"{resourceId}\"", innerException)
        {
        }
    }
}
