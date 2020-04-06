using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Organizr.Application.Planning.Common.Interfaces;
using Organizr.Domain.SharedKernel;

namespace Organizr.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string CurrentUserId => _httpContextAccessor.HttpContext?.User?.Identity?.Name;

        public IdentityService(IHttpContextAccessor httpContextAccessor)
        {
            Assert.Argument.NotNull(httpContextAccessor, nameof(httpContextAccessor));

            _httpContextAccessor = httpContextAccessor;
        }

        public Task<bool> UserExistsAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
