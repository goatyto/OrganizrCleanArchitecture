using FluentValidation;

namespace Organizr.Application.Planning.TodoLists.Commands.MoveTodoItem
{
    public class MoveTodoItemCommandValidator : AbstractValidator<MoveTodoItemCommand>
    {
        public MoveTodoItemCommandValidator()
        {
            RuleFor(c => c.TodoListId).NotEmpty();
            RuleFor(c => c.Id).GreaterThan(0);
            RuleFor(c => c.NewOrdinal).GreaterThan(0);
            RuleFor(c => c.NewSubListId).GreaterThan(0).When(c => c.NewSubListId.HasValue);
        }
    }
}
