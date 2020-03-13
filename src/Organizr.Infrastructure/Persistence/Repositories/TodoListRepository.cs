using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Infrastructure.Persistence.Repositories
{
    class TodoListRepository: ITodoListRepository
    {
        private readonly OrganizrContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public TodoListRepository(OrganizrContext context)
        {
            _context = context;
        }

        public async Task<TodoList> GetAsync(Guid id, Guid? userGroupId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _context.TodoLists.SingleAsync(l => l.Id == id && l.UserGroupId == userGroupId,
                cancellationToken);
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
