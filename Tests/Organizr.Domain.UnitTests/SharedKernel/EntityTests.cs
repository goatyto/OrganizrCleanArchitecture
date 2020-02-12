using System;
using System.Collections.Generic;
using FluentAssertions;
using Organizr.Domain.SharedKernel;
using Xunit;

namespace Organizr.Domain.UnitTests.SharedKernel
{
    public class EntityTests
    {
        public static readonly IEnumerable<object[]> EntityInvalidIds = new List<object[]>
        {
            new object[] {default(Guid)},
            new object[] {Guid.Empty}
        };

        [Theory, MemberData(nameof(EntityInvalidIds))]
        public void Constructor_InvalidId_ThrowEntityException(Guid id)
        {
            Func<EntityMock> construct = () => new EntityMock(id);

            construct.Should().Throw<ArgumentException>().And.ParamName.Should().Be(nameof(EntityMock.Id));
        }

        private class EntityMock : Entity<Guid>
        {
            public EntityMock(Guid id)
            {
                Id = id;
            }
        }
    }
}
