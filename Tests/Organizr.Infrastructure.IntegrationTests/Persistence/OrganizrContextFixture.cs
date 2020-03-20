using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Infrastructure.Persistence;

namespace Organizr.Infrastructure.IntegrationTests.Persistence
{
    public class OrganizrContextFixture : IDisposable
    {
        public OrganizrContext Context { get; }

        public OrganizrContextFixture()
        {
            var options = new DbContextOptionsBuilder<OrganizrContext>()
                .UseSqlite("DataSource=:memory:")
                //.UseSqlite("DataSource=TestDb.db")
                .Options;

            Context = new OrganizrContext(options);

            //Context.Database.EnsureDeleted();

            Context.Database.OpenConnection();
            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            Context?.Database?.CloseConnection();
            Context?.Database?.EnsureDeleted();
            Context?.Dispose();
        }
    }
}
