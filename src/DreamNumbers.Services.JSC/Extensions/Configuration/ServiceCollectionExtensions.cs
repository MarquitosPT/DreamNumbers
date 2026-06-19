using DreamNumbers.Services.JSC.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DreamNumbers.Services.JSC.Extensions.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJSCService(this IServiceCollection services)
        {
            // Register JSC services here in the future.
            services.AddHttpClient<EuroMillionsScraper>();
            services.AddScoped<EuroMillionsScraper>();

            return services;
        }
    }
}
