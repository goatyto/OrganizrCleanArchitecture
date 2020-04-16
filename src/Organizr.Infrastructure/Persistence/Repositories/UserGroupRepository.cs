using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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

        public async Task<UserGroup> GetAsync(Guid id, string memberUserId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _context.UserGroups
                .Include(ug => ug.Members)
                .SingleAsync(
                    ug => ug.Id == id && ug.Members.Any(m => m.UserId == memberUserId), 
                    cancellationToken);
        }

        public void Add(UserGroup userGroup)
        {
            _context.UserGroups.Add(userGroup);
        }

        public void Update(UserGroup userGroup)
        {
            _context.UserGroups.Update(userGroup);
        }
    }
}
