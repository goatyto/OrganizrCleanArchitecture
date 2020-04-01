using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;

namespace Organizr.Infrastructure.Persistence.Configuration
{
    public class TodoSubListConfiguration : IEntityTypeConfiguration<TodoSubList>
    {
        public void Configure(EntityTypeBuilder<TodoSubList> builder)
        {
            builder.ToTable(nameof(OrganizrContext.TodoSubLists), OrganizrContext.LISTS_SCHEMA);

            builder.Property<int>("DbId");
            builder.HasKey("DbId");

            builder.Ignore(sl => sl.DomainEvents);

            builder.Property(sl => sl.Title).IsRequired();
            builder.Property(sl => sl.Description);
            builder.Property(sl => sl.IsDeleted).IsRequired();
            builder.Property(sl => sl.Ordinal).IsRequired();

            builder.HasMany(sl => sl.Items)
                .WithOne()
                .HasForeignKey("ParentSubListId");
        }
    }
}
