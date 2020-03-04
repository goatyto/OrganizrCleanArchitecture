using FluentValidation;

namespace Organizr.Application.TodoLists.Commands.CreateTodoList
{
    public class CreateTodoListCommandValidator: AbstractValidator<CreateTodoListCommand>
    {
        public CreateTodoListCommandValidator()
        {
            RuleFor(c => c.Title).NotEmpty();
        }
    }
}
