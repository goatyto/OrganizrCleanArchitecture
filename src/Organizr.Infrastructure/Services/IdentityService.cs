using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;
using Organizr.Application.Planning.Common.Interfaces;

namespace Organizr.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string CurrentUserId => _httpContextAccessor.HttpContext?.User?.Identity?.Name;

        public IdentityService(IHttpContextAccessor httpContextAccessor)
        {
            Guard.Against.Null(httpContextAccessor, nameof(httpContextAccessor));

            _httpContextAccessor = httpContextAccessor;
        }

        public Task<bool> UserExistsAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
