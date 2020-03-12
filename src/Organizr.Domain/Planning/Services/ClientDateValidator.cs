using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.GuardClauses;
using Organizr.Domain.Guards;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Services
{
    public class ClientDateValidator
    {
        public bool IsDateBeforeClientToday(DateTime date, int clientTimeZoneOffsetInMinutes)
        {
            Guard.Against.NonUtcDateTime(date, nameof(date));

            return date.AddMinutes(-clientTimeZoneOffsetInMinutes) <
                   DateTime.UtcNow.AddMinutes(-clientTimeZoneOffsetInMinutes).Date;
        }
    }
}
