using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Organizr.Application.UnitTests
{
    public class DependencyInjectionTests
    {
        private readonly Mock<IServiceCollection> _serviceCollectionMock;

        public DependencyInjectionTests()
        {
            _serviceCollectionMock = new Mock<IServiceCollection>();
        }

        [Fact]
        public void AddApplication_ValidServiceCollection_DoesNotThrow()
        {
            _serviceCollectionMock.Object.Invoking(sc => sc.AddApplication()).Should().NotThrow();
        }

        [Fact]
        public void AddApplication_NullServiceCollection_ThrowsArgumentNullException()
        {
            ((IServiceCollection) null).Invoking(sc => sc.AddApplication()).Should().Throw<ArgumentNullException>().And
                .ParamName.Should().Be("services");
        }
    }
}
