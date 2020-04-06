using FluentValidation;

namespace Organizr.Application.Planning.UserGroups.CreateUserGroup
{
    public class CreateUserGroupCommandValidator : AbstractValidator<CreateUserGroupCommand>
    {
        public CreateUserGroupCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleForEach(c => c.GroupMemberIds).NotEmpty();
        }
    }
}
