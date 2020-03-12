using System;
using System.Collections.Generic;
using System.Text;
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

            builder.HasKey(ti => ti.Id);

            builder.Property(ti => ti.Id).UseHiLo("todoitemseq", OrganizrContext.LISTS_SCHEMA);
            builder.Property(ti => ti.TodoListId).IsRequired();
            builder.Property(ti => ti.Title).IsRequired();
            builder.Property(ti => ti.Description);
            builder.OwnsOne(ti => ti.Position, navigationBuilder => navigationBuilder.WithOwner());
            builder.Property(ti => ti.IsCompleted);
            builder.Property(ti => ti.IsDeleted);
            builder.Property(ti => ti.DueDateUtc);
        }
    }
}
