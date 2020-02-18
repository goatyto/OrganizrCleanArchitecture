using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Organizr.Domain.SharedKernel;
using Xunit;

namespace Organizr.Domain.UnitTests.SharedKernel
{
    public class ResourceEntityTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_InvalidParameters_ThrowArgumentException(string ownerId)
        {
            Func<ResourceMock> construct = () => new ResourceMock(ownerId);

            construct.Should().Throw<ArgumentException>().And.ParamName.Should().Be(nameof(ResourceMock.OwnerId));
        }

        private class ResourceMock : ResourceEntity
        {
            public ResourceMock(string ownerId)
            {
                OwnerId = ownerId;
            }
        }
    }
}
