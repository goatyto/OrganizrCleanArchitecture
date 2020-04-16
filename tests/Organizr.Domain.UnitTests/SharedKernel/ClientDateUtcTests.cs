using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Organizr.Domain.SharedKernel;
using Xunit;

namespace Organizr.Domain.UnitTests.SharedKernel
{
    public class ClientDateUtcTests
    {
        [Theory]
        [InlineData(-60)]
        [InlineData(0)]
        [InlineData(60)]
        public void Create_ValidData_ObjectInitialized(int clientTimeZoneOffsetInMinutes)
        {
            var date = DateTime.UtcNow.Date.AddMinutes(clientTimeZoneOffsetInMinutes);

            var clientDateUtc = ClientDateUtc.Create(date, clientTimeZoneOffsetInMinutes);

            clientDateUtc.Date.Should().Be(date);
            clientDateUtc.ClientTimeZoneOffsetInMinutes.Should().Be(clientTimeZoneOffsetInMinutes);
            clientDateUtc.IsBeforeClientToday.Should().Be(false);
        }

        [Fact]
        public void Create_NullData_ReturnsNullObject()
        {
            DateTime? date = null;
            int? clientTimeZoneOffsetInMinutes = null;

            var clientDateUtc = ClientDateUtc.Create(date, clientTimeZoneOffsetInMinutes);

            clientDateUtc.Should().BeNull();
        }

        [Fact]
        public void Create_DateWithTimeComponent_ThrowsArgumentException()
        {
            var date = DateTime.UtcNow.Date.AddTicks(1);
            var clientTimeZoneOffsetInMinutes = 0;

            Func<ClientDateUtc> sut = () => ClientDateUtc.Create(date, clientTimeZoneOffsetInMinutes);

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("date");
        }

        [Theory]
        [InlineData(-999)]
        [InlineData(999)]
        [InlineData(33)]
        public void Create_InvalidClientTimeZoneOffsetInMinutes_ThrowsArgumentException(int invalidClientTimeZoneOffsetInMinutes)
        {
            var date = DateTime.UtcNow.Date.AddMinutes(invalidClientTimeZoneOffsetInMinutes);

            Func<ClientDateUtc> sut = () => ClientDateUtc.Create(date, invalidClientTimeZoneOffsetInMinutes);

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("clientTimeZoneOffsetInMinutes");
        }

        [Fact]
        public void Create_NullClientTimeZoneOffsetInMinutes_ThrowsArgumentException()
        {
            int? nullClientTimeZoneOffsetInMinutes = null;
            var date = DateTime.UtcNow.Date;

            Func<ClientDateUtc> sut = () => ClientDateUtc.Create(date, nullClientTimeZoneOffsetInMinutes);

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("clientTimeZoneOffsetInMinutes");
        }
    }
}
