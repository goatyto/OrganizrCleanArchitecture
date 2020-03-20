using System;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.Planning.Common.Interfaces
{
    public interface IResourceAuthorizationService<in TResource> where TResource: Entity<Guid>, IAggregateRoot
    {
        bool CanRead(string userId, TResource resource);
        bool CanModify(string userId, TResource aggregateRoot);
        bool CanDelete(string userId, TResource aggregateRoot);
    }
}