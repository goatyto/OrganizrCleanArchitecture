using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Infrastructure.Persistence.Configuration
{
    public class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
    {
        public void Configure(EntityTypeBuilder<TodoList> builder)
        {
            builder.ToTable(nameof(OrganizrContext.TodoLists), OrganizrContext.LISTS_SCHEMA);

            builder.HasKey(tl => tl.Id);

            builder.Ignore(tl => tl.DomainEvents);

            builder.Property(tl => tl.Title).IsRequired();
            builder.Property(tl => tl.Description);
            
            builder.HasMany(tl => tl.SubLists).WithOne().HasPrincipalKey(tl => tl.Id).HasForeignKey("TodoListId");
            builder.HasMany(tl => tl.Items).WithOne().HasPrincipalKey(tl => tl.Id).HasForeignKey("TodoListId");
        }
    }
}