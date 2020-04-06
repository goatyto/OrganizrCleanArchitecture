using FluentValidation;

namespace Organizr.Application.Planning.TodoLists.Commands.AddTodoItem
{
    public class AddTodoItemCommandValidator : AbstractValidator<AddTodoItemCommand>
    {
        public AddTodoItemCommandValidator()
        {
            RuleFor(c => c.TodoListId).NotEmpty();
            RuleFor(c => c.SubListId).GreaterThan(0).When(c => c.SubListId.HasValue);
            RuleFor(c => c.Title).NotEmpty();

            When(c => c.DueDateUtc.HasValue, () =>
            {
                RuleFor(c => c.ClientTimeZoneOffsetInMinutes).NotNull();
            });
        }
    }
}
