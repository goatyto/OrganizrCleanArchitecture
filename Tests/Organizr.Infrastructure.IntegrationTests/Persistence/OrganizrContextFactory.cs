using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Organizr.Infrastructure.Persistence;

namespace Organizr.Infrastructure.IntegrationTests.Persistence
{
    public static class OrganizrContextFactory
    {
        public static OrganizrContext Create()
        {

            var options = new DbContextOptionsBuilder<OrganizrContext>()
                .UseSqlite("DataSource=:memory:")
                //.UseSqlite("DataSource=TestDb.db")
                .Options;

            var context = new OrganizrContext(options);

            context.Database.EnsureDeleted();

            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            return context;
        }
    }
}
