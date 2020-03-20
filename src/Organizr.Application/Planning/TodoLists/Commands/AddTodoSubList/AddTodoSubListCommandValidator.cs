using FluentValidation;

namespace Organizr.Application.Planning.TodoLists.Commands.AddTodoSubList
{
    public class AddTodoSubListCommandValidator: AbstractValidator<AddTodoSubListCommand>
    {
        public AddTodoSubListCommandValidator()
        {
            RuleFor(c => c.TodoListId).NotEmpty();
            RuleFor(c => c.Title).NotEmpty();
        }
    }
}
