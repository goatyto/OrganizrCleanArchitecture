using FluentValidation;

namespace Organizr.Application.Planning.UserGroups.RemoveMember
{
    public class RemoveMemberCommandValidator : AbstractValidator<RemoveMemberCommand>
    {
        public RemoveMemberCommandValidator()
        {
            RuleFor(c => c.UserGroupId).NotEmpty();
            RuleFor(c => c.MemberUserId).NotEmpty();
        }
    }
}
