using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Organizr.Domain.SharedKernel;

namespace Organizr.Infrastructure.Persistence.Repositories
{
    public class ResourceEntityRepository : IResourceEntityRepository
    {
        private readonly OrganizrContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public ResourceEntityRepository(OrganizrContext context)
        {
            _context = context;
        }

        public async Task<TResource> GetByIdAsync<TResource>(Guid id, CancellationToken cancellationToken = default(CancellationToken)) where TResource : ResourceEntity
        {
            return await _context.Set<TResource>().FindAsync(id, cancellationToken);
        }

        public void Update<TResource>(TResource resourceEntity) where TResource : ResourceEntity
        {
            _context.Entry(resourceEntity).State = EntityState.Modified;
        }
    }
}
