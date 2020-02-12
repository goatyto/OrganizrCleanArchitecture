using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Organizr.Infrastructure.Identity
{
    public class IsOwnerOrContributorRequirement : IAuthorizationRequirement
    {
    }
}
