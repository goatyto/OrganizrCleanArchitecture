using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.GuardClauses;

namespace Organizr.Domain.SharedKernel
{
    public class ClientDateUtc : ValueObject
    {
        public DateTime Date { get; }
        public int ClientTimeZoneOffsetInMinutes { get; }

        public bool IsBeforeClientToday => Date.AddMinutes(-ClientTimeZoneOffsetInMinutes) <
                                             DateTime.UtcNow.AddMinutes(-ClientTimeZoneOffsetInMinutes).Date;

        private ClientDateUtc(DateTime date, int clientTimeZoneOffsetInMinutes)
        {
            if (date.Kind != DateTimeKind.Utc)
                throw new ArgumentException("Date must be of kind Utc.", nameof(date));

            if(date > date.Date)
                throw new ArgumentException("Date must not have time component.", nameof(date));

            if (clientTimeZoneOffsetInMinutes % 60 != 0 && 
                clientTimeZoneOffsetInMinutes % 45 != 0 &&
                clientTimeZoneOffsetInMinutes % 30 != 0)
                throw new ArgumentException($"Invalid client timezone offset value: {clientTimeZoneOffsetInMinutes}.",
                    nameof(clientTimeZoneOffsetInMinutes));

            if(Math.Abs(clientTimeZoneOffsetInMinutes) > 60 * 24)
                throw new ArgumentException("Client timezone offset is out of range.", nameof(clientTimeZoneOffsetInMinutes));

            Date = date;
            ClientTimeZoneOffsetInMinutes = clientTimeZoneOffsetInMinutes;
        }

        public static ClientDateUtc Create(DateTime? date, int? clientTimeZoneOffsetInMinutes)
        {
            if (!date.HasValue)
                return null;

            Guard.Against.Null(clientTimeZoneOffsetInMinutes, nameof(clientTimeZoneOffsetInMinutes));

            return new ClientDateUtc(date.Value, clientTimeZoneOffsetInMinutes.Value);
        }

        public static implicit operator DateTime(ClientDateUtc clientDateUtc) => clientDateUtc.Date;
        public static implicit operator DateTime?(ClientDateUtc clientDateUtc) => clientDateUtc?.Date;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Date;
            yield return ClientTimeZoneOffsetInMinutes;
        }
    }
}
