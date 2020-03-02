﻿using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Organizr.Application.TodoLists.Commands.EditTodoSubList
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
