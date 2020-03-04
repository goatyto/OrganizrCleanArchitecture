using Ardalis.GuardClauses;
using FluentValidation;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.TodoLists.Commands.EditTodoItem
{
    public class EditTodoItemCommandValidator : AbstractValidator<EditTodoItemCommand>
    {
        private readonly IDateTime _dateTimeProvider;

        public EditTodoItemCommandValidator(IDateTime dateTimeProvider)
        {
            Guard.Against.Null(dateTimeProvider, nameof(dateTimeProvider));

            _dateTimeProvider = dateTimeProvider;

            RuleFor(c => c.TodoListId).NotEmpty();
            RuleFor(c => c.Id).GreaterThan(0);
            RuleFor(c => c.Title).NotEmpty();
            RuleFor(c => c.DueDate).GreaterThanOrEqualTo(_dateTimeProvider.Today).When(c => c.DueDate.HasValue);
        }
    }
}
