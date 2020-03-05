using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Organizr.Domain.Lists.Entities.TodoListAggregate;

namespace Organizr.Infrastructure.Persistence.Configuration
{
    public class TodoListConfiguration: IEntityTypeConfiguration<TodoList>
    {
        public void Configure(EntityTypeBuilder<TodoList> builder)
        {
            builder.HasKey(tl => tl.Id);

            builder.Property(tl => tl.Id).HasDefaultValueSql("newsequentialid()");
        }
    }
}