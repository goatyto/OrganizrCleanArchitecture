using System;
using FluentAssertions;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Organizr.Infrastructure.Identity;
using Xunit;

namespace Organizr.Infrastructure.IntegrationTests.Identity
{
    public class AppIdentityDbContextTests : IDisposable
    {
        private readonly AppIdentityDbContext _sut;

        public AppIdentityDbContextTests()
        {
            var options = new DbContextOptionsBuilder<AppIdentityDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            var operationalStoreOptionsMock = new Mock<IOptions<OperationalStoreOptions>>();
            operationalStoreOptionsMock.Setup(op => op.Value).Returns(new OperationalStoreOptions());

            _sut = new AppIdentityDbContext(options, operationalStoreOptionsMock.Object);

            _sut.Database.OpenConnection();
        }

        [Fact]
        public void Migrate_DoesNotThrow()
        {
            _sut.Database.Invoking(db => db.Migrate()).Should().NotThrow();
        }

        public void Dispose()
        {
            _sut?.Database?.CloseConnection();
            _sut?.Dispose();
        }
    }
}
