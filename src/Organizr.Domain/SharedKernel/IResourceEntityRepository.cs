using System;
using System.Threading;
using System.Threading.Tasks;

namespace Organizr.Domain.SharedKernel
{
    public interface IResourceEntityRepository
    {
        IUnitOfWork UnitOfWork { get; }

        Task<TResource> GetByIdAsync<TResource>(Guid id,
            CancellationToken cancellationToken = default(CancellationToken)) where TResource: ResourceEntity;

        void Update<TResource>(TResource resourceEntity) where TResource : ResourceEntity;
    }
}