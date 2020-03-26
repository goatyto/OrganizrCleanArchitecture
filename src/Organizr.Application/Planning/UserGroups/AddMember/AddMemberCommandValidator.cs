using FluentValidation;

namespace Organizr.Application.Planning.UserGroups.AddMember
{
    public class AddMemberCommandValidator : AbstractValidator<AddMemberCommand>
    {
        public AddMemberCommandValidator()
        {
            RuleFor(c => c.UserGroupId).NotEmpty();
            RuleFor(c => c.UserId).NotEmpty();
        }
    }
}
