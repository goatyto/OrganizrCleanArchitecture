using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Moq;
using Organizr.Domain.Planning.Services;
using Organizr.Domain.SharedKernel;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning
{
    public class ClientDateValidatorTests
    {
        public static IEnumerable<object[]> IsDateBeforeClientTodayTestData = new List<object[]>
        {
            new object[] {-60, -1, true},
            new object[] {0, -1, true},
            new object[] {60, -1, true},

            new object[] {-60, 0, false},
            new object[] {0, 0, false},
            new object[] {60, 0, false},

            new object[] {-60, 1, false},
            new object[] {0, 1, false},
            new object[] {60, 1, false},
        };

        [Theory, MemberData(nameof(IsDateBeforeClientTodayTestData))]
        public void IsDateBeforeClientToday_ValidData_ReturnsExpectedResult(int clientTimeZoneOffsetInMinutes, int clientDateAddDays, bool expectedResult)
        {
            var clientDate = DateTime.UtcNow
                // normalize date to client timezone
                .AddMinutes(-clientTimeZoneOffsetInMinutes)
                // remove time component
                .Date
                // adjust days to simulate datepicker value
                .AddDays(clientDateAddDays)
                // reverse normalization to UTC time
                .AddMinutes(clientTimeZoneOffsetInMinutes);

            var sut = new ClientDateValidator();

            var result = sut.IsDateBeforeClientToday(clientDate, clientTimeZoneOffsetInMinutes);

            result.Should().Be(expectedResult);
        }

        [Fact]
        public void IsDateBeforeClientToday_NonUtcClientDate_ThrowsArgumentException()
        {
            var clientDate = DateTime.Today;

            var sut = new ClientDateValidator();

            sut.Invoking(s => s.IsDateBeforeClientToday(clientDate, It.IsAny<int>())).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("date");
        }
    }
}
