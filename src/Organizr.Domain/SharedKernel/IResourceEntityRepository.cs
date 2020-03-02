using System;
using System.Threading;
using System.Threading.Tasks;

namespace Organizr.Domain.SharedKernel
{
    public interface IResourceEntityRepository
    {
        IUnitOfWork UnitOfWork { get; }

        Task<ResourceEntity> GetByIdAsync(Guid id,
            CancellationToken cancellationToken = default(CancellationToken));

        void Update(ResourceEntity resourceEntity);
    }
}