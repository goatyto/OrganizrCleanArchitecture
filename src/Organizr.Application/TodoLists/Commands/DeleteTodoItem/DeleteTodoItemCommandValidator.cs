﻿using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Organizr.Application.TodoLists.Commands.DeleteTodoItem
{
    public class DeleteTodoItemCommandValidator: AbstractValidator<DeleteTodoItemCommand>
    {
        public DeleteTodoItemCommandValidator()
        {
            RuleFor(c => c.TodoListId).NotEmpty();
            RuleFor(c => c.Id).GreaterThan(0);
        }
    }
}