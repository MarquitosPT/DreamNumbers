using DreamNumbers.Services.EUDreams.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DreamNumbers.Services.EUDreams.Extensions.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEUDreamsService(this IServiceCollection services)
        {
            // Register EuroDreams services here in the future.
            services.AddHttpClient<EuroDreamsScraper>();
            services.AddScoped<EuroDreamsScraper>();

            return services;
        }
    }
}
