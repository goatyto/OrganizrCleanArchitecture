using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            Guard.Against.Null(services, nameof(services));

            // TODO: Add services

            return services;
        }
    }
}
