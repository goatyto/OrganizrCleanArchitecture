using FluentValidation;

namespace Organizr.Application.Planning.TodoLists.Commands.CreateTodoList
{
    public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
    {
        public CreateTodoListCommandValidator()
        {
            RuleFor(c => c.Title).NotEmpty();
        }
    }
}
