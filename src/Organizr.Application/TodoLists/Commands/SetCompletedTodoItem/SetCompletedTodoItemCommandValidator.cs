using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Organizr.Application.TodoLists.Commands.SetCompletedTodoItem
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
