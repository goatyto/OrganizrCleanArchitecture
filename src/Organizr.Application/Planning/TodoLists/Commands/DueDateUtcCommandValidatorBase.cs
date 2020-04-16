using System;
using FluentValidation;

namespace Organizr.Application.Planning.TodoLists.Commands
{
    public abstract class DueDateUtcCommandValidatorBase<TCommand> : AbstractValidator<TCommand> where TCommand: DueDateUtcCommandBase 
    {
        private const int TimeZoneOffsetLowerBound = -12 * 60;
        private const int TimeZoneOffsetUpperBound = 14 * 60;
        
        protected DueDateUtcCommandValidatorBase()
        {
            When(c => c.DueDateUtc.HasValue, () =>
            {
                RuleFor(c => c.ClientTimeZoneOffsetInMinutes)
                    .NotNull()
                    .Must(clientTimeZoneOffsetInMinutes => 
                        !clientTimeZoneOffsetInMinutes.HasValue ||
                        clientTimeZoneOffsetInMinutes.Value > TimeZoneOffsetLowerBound &&
                        clientTimeZoneOffsetInMinutes.Value < TimeZoneOffsetUpperBound &&
                        (clientTimeZoneOffsetInMinutes.Value % 60 == 0 ||
                         clientTimeZoneOffsetInMinutes.Value % 45 == 0 ||
                         clientTimeZoneOffsetInMinutes.Value % 30 == 0))
                    .WithMessage("Value \"{PropertyValue}\" for property \"{PropertyName}\" is invalid.");
                
                RuleFor(c => c.DueDateUtc)
                    .Must(dueDateUtc => dueDateUtc.Value.Kind == DateTimeKind.Utc)
                    .WithMessage("{PropertyName} must be of kind UTC.")
                    .Must((command, dueDateUtc) =>
                    {
                        if (!command.ClientTimeZoneOffsetInMinutes.HasValue)
                            return true;

                        var normalizedClientDate =
                            dueDateUtc.Value.AddMinutes(-command.ClientTimeZoneOffsetInMinutes.Value);
                        return normalizedClientDate == normalizedClientDate.Date;
                    })
                    .WithMessage("{PropertyName} must not have time component.")
                    .Must((command, dueDateUtc) => !command.ClientTimeZoneOffsetInMinutes.HasValue ||
                        dueDateUtc.Value.AddMinutes(-command.ClientTimeZoneOffsetInMinutes.Value) >= DateTime.UtcNow
                            .AddMinutes(-command.ClientTimeZoneOffsetInMinutes.Value).Date)
                    .WithMessage("{PropertyName} must be greater or equal than today.");

            });
        }
    }
}
