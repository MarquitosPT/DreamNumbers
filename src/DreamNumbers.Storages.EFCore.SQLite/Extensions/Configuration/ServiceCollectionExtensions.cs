using DreamNumbers.Storages.EFCore.Extensions.Configuration;
using DreamNumbers.Storages.EFCore.SQLite.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DreamNumbers.Storages.EFCore.SQLite.Extensions.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModuleDbContext(this IServiceCollection services, string connectionString)
        {
            return services.AddDreamNumbersDbContext(options => options.UseSqlite(connectionString));
        }

        public static IServiceCollection AddDreamNumbersDbContext(this IServiceCollection services, Action<DbContextOptionsBuilder>? optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        {
            return services.AddDreamNumbersDbContext<DreamNumbersDbContext>(optionsAction, contextLifetime, optionsLifetime);
        }

        public static IServiceCollection AddDreamNumbersStorage(this IServiceCollection services, string connectionString)
        {
            return services.AddDreamNumbersStorage(options => options.UseSqlite(connectionString));
        }

        public static IServiceCollection AddDreamNumbersStorage(this IServiceCollection services, Action<DbContextOptionsBuilder>? optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        {
            return services.AddDreamNumbersStorage<DreamNumbersDbContext>(optionsAction, contextLifetime, optionsLifetime);
        }
    }
}
