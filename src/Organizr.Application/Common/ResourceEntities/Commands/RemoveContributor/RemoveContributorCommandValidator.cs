using FluentValidation;

namespace Organizr.Application.Common.ResourceEntities.Commands.RemoveContributor
{
    public class RemoveContributorCommandValidator: AbstractValidator<RemoveContributorCommand>
    {
        public RemoveContributorCommandValidator()
        {
            RuleFor(c => c.ResourceId).NotEmpty();
            RuleFor(c => c.ContributorId).NotEmpty();
        }
    }
}
