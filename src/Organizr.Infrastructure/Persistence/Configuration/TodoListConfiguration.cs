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

            builder.Property(tl => tl.Id).HasDefaultValueSql("newsequentialid()");
            builder.Property(tl => tl.Title).IsRequired();
            builder.Property(tl => tl.Description);
            builder.Metadata.FindNavigation(nameof(TodoList.SubLists)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(TodoList.Items)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}