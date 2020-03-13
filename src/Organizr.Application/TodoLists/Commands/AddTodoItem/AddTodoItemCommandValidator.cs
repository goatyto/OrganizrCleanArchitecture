using System;
using Ardalis.GuardClauses;
using FluentValidation;
using Organizr.Domain.Planning.Services;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.TodoLists.Commands.AddTodoItem
{
    public class AddTodoItemCommandValidator : AbstractValidator<AddTodoItemCommand>
    {
        private readonly ClientDateValidator _clientDateValidator;

        public AddTodoItemCommandValidator(ClientDateValidator clientDateValidator)
        {
            Guard.Against.Null(clientDateValidator, nameof(clientDateValidator));

            _clientDateValidator = clientDateValidator;

            RuleFor(c => c.TodoListId).NotEmpty();
            RuleFor(c => c.SubListId).GreaterThan(0).When(c => c.SubListId.HasValue);
            RuleFor(c => c.Title).NotEmpty();

            When(c => c.DueDateUtc.HasValue, () =>
            {
                RuleFor(c => c.ClientTimeZoneOffsetInMinutes).NotNull();
                RuleFor(c => c.DueDateUtc).Must((c, dueDateUtc) => dueDateUtc.Value == dueDateUtc.Value.Date)
                    .WithMessage("Todo item due date cannot have a time component.");
                RuleFor(c => c.DueDateUtc).Must((c, dueDateUtc) => dueDateUtc.Value.Kind == DateTimeKind.Utc)
                    .WithMessage("Todo item due date must be UTC.");
                RuleFor(c => c.DueDateUtc).Must((c, dueDateUtc) =>
                    !_clientDateValidator.IsDateBeforeClientToday(dueDateUtc.Value, c.ClientTimeZoneOffsetInMinutes.Value))
                    .WithMessage("Todo item due date must be in the future.");
            });
        }
    }
}
