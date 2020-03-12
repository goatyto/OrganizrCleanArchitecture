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

            builder.HasKey(sl => sl.Id);

            builder.Property(sl => sl.Id).UseHiLo("todosublistseq", OrganizrContext.LISTS_SCHEMA);
            builder.Property(sl => sl.TodoListId).IsRequired();
            builder.Property(sl => sl.Title).IsRequired();
            builder.Property(sl => sl.Description);
            builder.Property(sl => sl.Ordinal).IsRequired();
            builder.Property(sl => sl.IsDeleted);
        }
    }
}
