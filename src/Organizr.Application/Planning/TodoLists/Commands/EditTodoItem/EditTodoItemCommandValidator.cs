using FluentValidation;

namespace Organizr.Application.Planning.TodoLists.Commands.EditTodoItem
{
    public class EditTodoItemCommandValidator : AbstractValidator<EditTodoItemCommand>
    {
        public EditTodoItemCommandValidator()
        {
            RuleFor(c => c.TodoListId).NotEmpty();
            RuleFor(c => c.Id).GreaterThan(0);
            RuleFor(c => c.Title).NotEmpty();

            When(c => c.DueDateUtc.HasValue, () =>
            {
                RuleFor(c => c.ClientTimeZoneOffsetInMinutes).NotNull();
            });
        }
    }
}
