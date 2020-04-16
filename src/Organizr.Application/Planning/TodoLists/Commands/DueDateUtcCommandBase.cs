using System;
using MediatR;

namespace Organizr.Application.Planning.TodoLists.Commands
{
    public abstract class DueDateUtcCommandBase : IRequest
    {
        public DateTime? DueDateUtc { get; }
        public int? ClientTimeZoneOffsetInMinutes { get; }

        protected DueDateUtcCommandBase(DateTime? dueDateUtc, int? clientTimeZoneOffsetInMinutes)
        {
            DueDateUtc = dueDateUtc;
            ClientTimeZoneOffsetInMinutes = clientTimeZoneOffsetInMinutes;
        }
    }
}
