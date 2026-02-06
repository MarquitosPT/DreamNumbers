using DreamNumbers.Storages.EFCore.Extensions.Configuration;
using DreamNumbers.Storages.EFCore.SQLite.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DreamNumbers.Storages.EFCore.SQLite.Extensions.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqliteModuleDbContext(this IServiceCollection services, string connectionString)
        {
            return services.AddSqliteDreamNumbersDbContext(options => options.UseSqlite(connectionString));
        }

        public static IServiceCollection AddSqliteDreamNumbersDbContext(this IServiceCollection services, Action<DbContextOptionsBuilder>? optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        {
            return services.AddDreamNumbersDbContext<DreamNumbersDbContext>(optionsAction, contextLifetime, optionsLifetime);
        }

        public static IServiceCollection AddSqliteDreamNumbersStorage(this IServiceCollection services, string connectionString)
        {
            return services.AddSqliteDreamNumbersStorage(options => options.UseSqlite(connectionString));
        }

        public static IServiceCollection AddSqliteDreamNumbersStorage(this IServiceCollection services, Action<DbContextOptionsBuilder>? optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        {
            return services.AddDreamNumbersStorage<DreamNumbersDbContext>(optionsAction, contextLifetime, optionsLifetime);
        }
    }
}
