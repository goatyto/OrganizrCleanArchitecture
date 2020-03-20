using FluentValidation;

namespace Organizr.Application.Planning.TodoLists.Commands.EditTodoSubList
{
    public class EditTodoSubListCommandValidator : AbstractValidator<EditTodoSubListCommand>
    {
        public EditTodoSubListCommandValidator()
        {
            RuleFor(c => c.TodoListId).NotEmpty();
            RuleFor(c => c.Id).GreaterThan(0);
            RuleFor(c => c.Title).NotEmpty();
        }
    }
}
