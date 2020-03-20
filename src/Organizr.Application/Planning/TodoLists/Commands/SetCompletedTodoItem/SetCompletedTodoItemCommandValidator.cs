using FluentValidation;

namespace Organizr.Application.Planning.TodoLists.Commands.SetCompletedTodoItem
{
    public class SetCompletedTodoItemCommandValidator : AbstractValidator<SetCompletedTodoItemCommand>
    {
        public SetCompletedTodoItemCommandValidator()
        {
            RuleFor(c => c.TodoListId).NotEmpty();
            RuleFor(c => c.Id).GreaterThan(0);
        }
    }
}
