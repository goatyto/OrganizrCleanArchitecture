using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<UserGroup> GetAsync(UserGroupId id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _context.UserGroups.Include(ug => ug.Members)
                .SingleAsync(ug => ug.UserGroupId == id, cancellationToken);
        }

        public void Add(UserGroup userGroup)
        {
            _context.UserGroups.Add(userGroup);
        }

        public void Update(UserGroup userGroup)
        {
            throw new NotImplementedException();
        }
    }
}
