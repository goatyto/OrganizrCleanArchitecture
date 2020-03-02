using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.GuardClauses;
using FluentValidation;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.TodoLists.Commands.EditTodoList
{
    public class EditTodoListCommandValidator: AbstractValidator<EditTodoListCommand>
    {
        private readonly IDateTime _dateTimeProvider;

        public EditTodoListCommandValidator(IDateTime dateTimeProvider)
        {
            Guard.Against.Null(dateTimeProvider, nameof(dateTimeProvider));

            _dateTimeProvider = dateTimeProvider;

            RuleFor(c => c.Id).NotEmpty();
            RuleFor(c => c.Title).NotEmpty();
        }
    }
}
