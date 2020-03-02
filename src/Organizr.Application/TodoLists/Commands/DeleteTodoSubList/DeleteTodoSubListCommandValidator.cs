using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Organizr.Application.TodoLists.Commands.DeleteTodoSubList
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
