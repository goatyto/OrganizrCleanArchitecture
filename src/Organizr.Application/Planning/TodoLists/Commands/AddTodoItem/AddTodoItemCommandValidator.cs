using System;
using FluentValidation;

namespace Organizr.Application.Planning.TodoLists.Commands.AddTodoItem
{
    public class AddTodoItemCommandValidator : DueDateUtcCommandValidatorBase<AddTodoItemCommand>
    {
        public AddTodoItemCommandValidator()
        {
            RuleFor(c => c.TodoListId).NotEmpty();
            RuleFor(c => c.SubListId).GreaterThan(0).When(c => c.SubListId.HasValue);
            RuleFor(c => c.Title).NotEmpty();
        }
    }
}
