using Adapter.Core;
using Adapter.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Adapter.Extensions
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInertia(this IServiceCollection services)
        {
            services.AddSingleton<IResultFactory, ResultFactory>();
            return services;
        }
    }
}