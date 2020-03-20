using FluentValidation;

namespace Organizr.Application.Planning.TodoLists.Commands.DeleteTodoSubList
{
    public class DeleteTodoSubListCommandValidator: AbstractValidator<DeleteTodoSubListCommand>
    {
        public DeleteTodoSubListCommandValidator()
        {
            RuleFor(c => c.TodoListId).NotEmpty();
            RuleFor(c => c.Id).GreaterThan(0);
        }
    }
}
