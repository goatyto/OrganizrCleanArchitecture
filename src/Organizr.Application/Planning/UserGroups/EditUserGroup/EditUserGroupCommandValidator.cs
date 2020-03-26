using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Organizr.Application.Planning.UserGroups.EditUserGroup
{
    public class EditUserGroupCommandValidator : AbstractValidator<EditUserGroupCommand>
    {
        public EditUserGroupCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
            RuleFor(c => c.Name).NotEmpty();
        }
    }
}
