using Microsoft.Extensions.DependencyInjection;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            Assert.Argument.NotNull(services, nameof(services));

            // TODO: Add services

            return services;
        }
    }
}
