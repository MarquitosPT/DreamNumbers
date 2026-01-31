using DreamNumbers.Storages.EFCore.DbContexts;
using DreamNumbers.Storages.EFCore.Storages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DreamNumbers.Storages.EFCore.Extensions.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDreamNumbersDbContext<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder>? optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where TContext : DreamNumbersDbContext<TContext>
        {
            services.AddDbContext<TContext>(optionsAction, contextLifetime, optionsLifetime);

            return services;
        }

        public static IServiceCollection AddDreamNumbersStorage<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder>? optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where TContext : DreamNumbersDbContext<TContext>
        {
            services.AddDreamNumbersDbContext<TContext>(optionsAction, contextLifetime, optionsLifetime);

            services.AddScoped<IDrawStorage, DrawStorage<TContext>>();

            return services;
        }
    }
}
