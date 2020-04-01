using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Infrastructure.Persistence.Configuration
{
    public class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
    {
        public void Configure(EntityTypeBuilder<TodoList> builder)
        {
            builder.ToTable(nameof(OrganizrContext.TodoLists), OrganizrContext.LISTS_SCHEMA);

            builder.HasKey(tl => tl.TodoListId);
            builder.Property(tl => tl.TodoListId).HasConversion(todoListId => todoListId.Id, id => new TodoListId(id));

            builder.Ignore(tl => tl.DomainEvents);

            builder.OwnsOne(tl => tl.CreatorUser, creatorUserBuilder =>
            {
                creatorUserBuilder.Property(cub => cub.UserId).IsRequired();
            });

            builder.HasOne<UserGroup>().WithMany().HasForeignKey(tl => tl.UserGroupId);
            builder.Property(tl => tl.UserGroupId)
                .HasConversion(new UserGroupIdConverter());

            builder.Property(tl => tl.Title).IsRequired();
            builder.Property(tl => tl.Description);
            
            builder.HasMany(tl => tl.SubLists).WithOne().HasForeignKey("ParentListId");
            builder.HasMany(tl => tl.Items).WithOne().HasForeignKey("ParentListId");
        }
    }
}