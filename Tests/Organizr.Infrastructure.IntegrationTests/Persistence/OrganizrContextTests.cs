using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Organizr.Infrastructure.Persistence;

namespace Organizr.Infrastructure.IntegrationTests.Persistence
{
    public class OrganizrContextTests
    {
        private readonly OrganizrContext _sut;

        public OrganizrContextTests()
        {
            var options = new DbContextOptionsBuilder<OrganizrContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _sut = new OrganizrContext(options);
        }
    }
}
