using System;

namespace Organizr.Domain.SharedKernel
{
    public abstract class ResourceExceptionBase<TResource> : Exception where TResource : Entity<Guid>, IAggregateRoot
    {
        public Guid ResourceId { get; }

        protected ResourceExceptionBase(Guid resourceId, string message, Exception innerException = null)
        {
            ResourceId = resourceId;
        }
    }

    public class ResourceNotFoundException<TResource> : ResourceExceptionBase<TResource> where TResource : Entity<Guid>, IAggregateRoot
    {
        public ResourceNotFoundException(Guid resourceId, Exception innerException = null) : base(resourceId,
            $"Resource of type {nameof(TResource)} with id {resourceId} was not found.", innerException)
        {

        }
    }
}
