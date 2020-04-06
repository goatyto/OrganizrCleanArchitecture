﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;

namespace Organizr.Infrastructure.Persistence.Configuration
{
    public class UserGroupMemberConfiguration : IEntityTypeConfiguration<UserGroupMember>
    {
        public void Configure(EntityTypeBuilder<UserGroupMember> builder)
        {
            builder.ToTable(nameof(OrganizrContext.UserGroupMemberships), OrganizrContext.USERS_SCHEMA);

            builder.Property<int>("DbId");
            builder.HasKey("DbId");

            builder.Property(ugm => ugm.UserId).IsRequired();
        }
    }
}
