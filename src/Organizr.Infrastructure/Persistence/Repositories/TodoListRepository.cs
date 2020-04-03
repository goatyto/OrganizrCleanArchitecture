using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Infrastructure.Persistence.Repositories
{
    public class TodoListRepository : ITodoListRepository
    {
        private readonly OrganizrContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public TodoListRepository(OrganizrContext context)
        {
            _context = context;
        }

        public async Task<TodoList> GetOwnAsync(Guid id, string creatorUserId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _context
                .TodoLists
                .Include(tl => tl.SubLists)
                .Include(tl => tl.Items)
                .SingleAsync(l => l.Id == id && l.CreatorUserId == creatorUserId, cancellationToken);
        }

        public async Task<TodoList> GetSharedAsync(Guid id, string memberUserId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _context
                .TodoLists
                .Join(_context.UserGroups.Where(ug => ug.Members.Any(m => m.UserId == memberUserId)), 
                    tl => tl.UserGroupId,
                    ug => ug.Id, 
                    (tl, ug) => tl)
                .Include(tl => tl.SubLists)
                .Include(tl => tl.Items)
                .SingleAsync(l => l.Id == id, cancellationToken);
        }

        public void Add(TodoList todoList)
        {
            _context.TodoLists.Add(todoList);
        }

        public void Update(TodoList todoList)
        {
            _context.Entry(todoList).State = EntityState.Modified;
        }
    }
}
