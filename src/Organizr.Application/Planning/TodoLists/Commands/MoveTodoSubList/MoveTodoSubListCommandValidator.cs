using FluentValidation;

namespace Organizr.Application.Planning.TodoLists.Commands.MoveTodoSubList
{
    public class MoveTodoSubListCommandValidator : AbstractValidator<MoveTodoSubListCommand>
    {
        public MoveTodoSubListCommandValidator()
        {
            RuleFor(c => c.TodoListId).NotEmpty();
            RuleFor(c => c.Id).GreaterThan(0);
            RuleFor(c => c.NewOrdinal).GreaterThan(0);
        }
    }
}
