using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Organizr.Application.Planning.TodoLists.Commands.CreateTodoList;

namespace Organizr.Application.Planning.UserGroups.CreateSharedTodoList
{
    public class CreateSharedTodoListCommandValidator : AbstractValidator<CreateSharedTodoListCommand>
    {
        public CreateSharedTodoListCommandValidator()
        {
            RuleFor(c => c.UserGroupId).NotEmpty();
            RuleFor(c => c.Title).NotEmpty();
        }
    }
}
