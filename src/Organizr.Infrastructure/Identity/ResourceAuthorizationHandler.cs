using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Organizr.Domain.SharedKernel;

namespace Organizr.Infrastructure.Identity
{
    class ResourceAuthorizationHandler: AuthorizationHandler<IsOwnerOrContributorRequirement, ResourceEntity>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsOwnerOrContributorRequirement requirement,
            ResourceEntity resource)
        {
            if (context.User.HasClaim(c =>
                (c.Type == "Owner" || c.Type == "Contributor") && c.Value == resource.Id.ToString()))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
