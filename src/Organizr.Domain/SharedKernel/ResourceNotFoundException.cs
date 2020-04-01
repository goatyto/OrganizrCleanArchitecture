using System;
using System.Collections.Generic;
using System.Text;

namespace Organizr.Domain.SharedKernel
{
    public abstract class ResourceExceptionBase<TResource> : Exception where TResource : IAggregateRoot
    {
        public object ResourceId { get; }

        protected ResourceExceptionBase(object resourceId, string message, Exception innerException = null)
        {
            ResourceId = resourceId;
        }
    }

    public class ResourceNotFoundException<TResource> : ResourceExceptionBase<TResource> where TResource : IAggregateRoot
    {
        public ResourceNotFoundException(object resourceId, Exception innerException = null) : base(resourceId,
            $"Resource of type {nameof(TResource)} with id {resourceId} was not found.", innerException)
        {

        }
    }
}
