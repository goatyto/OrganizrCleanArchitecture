using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;

namespace Organizr.Infrastructure.Persistence.Configuration
{
    public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
    {
        public void Configure(EntityTypeBuilder<UserGroup> builder)
        {
            builder.ToTable(nameof(OrganizrContext.UserGroups), OrganizrContext.USERS_SCHEMA);

            builder.HasKey(ug => ug.Id);

            builder.Ignore(ug => ug.DomainEvents);

            builder.Property(ug => ug.Name).IsRequired();
            builder.Property(ug => ug.Description);
            builder.Property(ug => ug.CreatorUserId).IsRequired();

            builder.HasMany(ug => ug.Members).WithOne().HasForeignKey("UserGroupId");
            builder.HasMany<TodoList>().WithOne();
        }
    }
}
