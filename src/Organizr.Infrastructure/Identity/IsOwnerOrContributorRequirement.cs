using Microsoft.AspNetCore.Authorization;

namespace Organizr.Infrastructure.Identity
{
    public class IsOwnerOrContributorRequirement : IAuthorizationRequirement
    {
    }
}
