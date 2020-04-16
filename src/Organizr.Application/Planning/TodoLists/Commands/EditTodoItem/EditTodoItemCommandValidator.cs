using FluentValidation;

namespace Organizr.Application.Planning.TodoLists.Commands.EditTodoItem
{
    public class EditTodoItemCommandValidator : DueDateUtcCommandValidatorBase<EditTodoItemCommand>
    {
        public EditTodoItemCommandValidator()
        {
            RuleFor(c => c.TodoListId).NotEmpty();
            RuleFor(c => c.Id).GreaterThan(0);
            RuleFor(c => c.Title).NotEmpty();
        }
    }
}
