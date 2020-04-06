using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;

namespace Organizr.Infrastructure.Persistence.Configuration
{
    public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
    {
        public void Configure(EntityTypeBuilder<TodoItem> builder)
        {
            builder.ToTable(nameof(OrganizrContext.TodoItems), OrganizrContext.LISTS_SCHEMA);

            builder.Property<int>("DbId");
            builder.HasKey("DbId");

            builder.Ignore(ti => ti.DomainEvents);

            builder.Property(ti => ti.Title).IsRequired();
            builder.Property(ti => ti.Description);
            builder.Property(ti => ti.Ordinal).IsRequired();
            builder.Property(ti => ti.IsCompleted).IsRequired();
            builder.Property(ti => ti.IsDeleted).IsRequired();
            builder.Property(ti => ti.DueDateUtc);
        }
    }
}
