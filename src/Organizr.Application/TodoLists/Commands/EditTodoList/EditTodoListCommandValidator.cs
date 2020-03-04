using Ardalis.GuardClauses;
using FluentValidation;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.TodoLists.Commands.EditTodoList
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
