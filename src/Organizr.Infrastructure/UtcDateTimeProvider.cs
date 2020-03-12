using System;
using Organizr.Domain.SharedKernel;

namespace Organizr.Infrastructure
{
    public class UtcDateTimeProvider : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
        public DateTime Today => DateTime.UtcNow.Date;
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
