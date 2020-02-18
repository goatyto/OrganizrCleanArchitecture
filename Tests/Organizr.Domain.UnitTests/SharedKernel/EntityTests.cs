using System;
using System.Collections.Generic;
using FluentAssertions;
using Organizr.Domain.SharedKernel;
using Xunit;

namespace Organizr.Domain.UnitTests.SharedKernel
{
    public class EntityTests
    {
        private class EntityStub : Entity<Guid>
        {
            public EntityStub(Guid id)
            {
                Id = id;
            }
        }
    }
}
