using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Organizr.Domain.SharedKernel;
using Organizr.Infrastructure.Persistence.Configuration;

namespace Organizr.Infrastructure.Persistence
{
    public class OrganizrContext: DbContext, IUnitOfWork
    {
        public const string USERS_SCHEMA = "Users";
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserGroupMember> UserGroupMemberships { get; set; }

        public const string LISTS_SCHEMA = "Lists";
        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<TodoSubList> TodoSubLists { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }

        public OrganizrContext(DbContextOptions<OrganizrContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserGroupConfiguration());
            modelBuilder.ApplyConfiguration(new UserGroupMembershipConfiguration());

            modelBuilder.ApplyConfiguration(new TodoListConfiguration());
            modelBuilder.ApplyConfiguration(new TodoSubListConfiguration());
            modelBuilder.ApplyConfiguration(new TodoItemConfiguration());
        }

        async Task IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await SaveChangesAsync(cancellationToken);
        }
    }
}
