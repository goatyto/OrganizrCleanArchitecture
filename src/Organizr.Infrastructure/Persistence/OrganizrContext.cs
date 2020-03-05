using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Organizr.Domain.Lists.Entities.TodoListAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Infrastructure.Persistence
{
    public class OrganizrContext: DbContext, IUnitOfWork
    {
        public OrganizrContext()
        {
            
        }

        public DbSet<TodoList> TodoLists { get; }
        public DbSet<TodoSubList> TodoSubLists { get; }
        public DbSet<TodoItem> TodoItems { get; }

        async Task IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await SaveChangesAsync(cancellationToken);
        }
    }
}
