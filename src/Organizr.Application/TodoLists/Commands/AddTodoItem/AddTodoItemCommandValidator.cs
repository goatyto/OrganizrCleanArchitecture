using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.GuardClauses;
using FluentValidation;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.TodoLists.Commands.AddTodoItem
{
    public class AddTodoItemCommandValidator : AbstractValidator<AddTodoItemCommand>
    {
        private readonly IDateTime _dateTimeProvider;

        public AddTodoItemCommandValidator(IDateTime dateTimeProvider)
        {
            Guard.Against.Null(dateTimeProvider, nameof(dateTimeProvider));

            _dateTimeProvider = dateTimeProvider;

            RuleFor(c => c.TodoListId).NotEmpty();
            RuleFor(c => c.SubListId).GreaterThan(0).When(c => c.SubListId.HasValue);
            RuleFor(c => c.Title).NotEmpty();
            RuleFor(c => c.DueDate).GreaterThanOrEqualTo(_dateTimeProvider.Today).When(c => c.DueDate.HasValue);
        }
    }
}
