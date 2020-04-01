using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Infrastructure.Persistence.Repositories
{
    public class UserGroupRepository : IUserGroupRepository
    {
        private readonly OrganizrContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public UserGroupRepository(OrganizrContext context)
        {
            _context = context;
        }

        public Task<UserGroup> GetAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public void Add(UserGroup userGroup)
        {
            throw new NotImplementedException();
        }

        public void Update(UserGroup userGroup)
        {
            throw new NotImplementedException();
        }
    }
}
