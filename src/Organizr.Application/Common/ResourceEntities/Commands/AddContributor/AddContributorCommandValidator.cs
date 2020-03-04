using FluentValidation;

namespace Organizr.Application.Common.ResourceEntities.Commands.AddContributor
{
    public class AddContributorCommandValidator : AbstractValidator<AddContributorCommand>
    {
        public AddContributorCommandValidator()
        {
            RuleFor(c => c.ResourceId).NotEmpty();
            RuleFor(c => c.ContributorId).NotEmpty();
        }
    }
}
