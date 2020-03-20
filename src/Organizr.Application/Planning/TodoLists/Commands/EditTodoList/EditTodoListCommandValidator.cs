using FluentValidation;

namespace Organizr.Application.Planning.TodoLists.Commands.EditTodoList
{
    public class EditTodoListCommandValidator: AbstractValidator<EditTodoListCommand>
    {
        public EditTodoListCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
            RuleFor(c => c.Title).NotEmpty();
        }
    }
}
