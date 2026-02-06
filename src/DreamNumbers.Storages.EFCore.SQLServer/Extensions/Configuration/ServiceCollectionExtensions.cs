using DreamNumbers.Storages.EFCore.Extensions.Configuration;
using DreamNumbers.Storages.EFCore.SQLServer.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DreamNumbers.Storages.EFCore.SQLServer.Extensions.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureSqlModuleDbContext(this IServiceCollection services, string connectionString)
        {
            return services.AddAzureSqlDreamNumbersDbContext(options => options.UseAzureSql(connectionString));
        }

        public static IServiceCollection AddAzureSqlDreamNumbersDbContext(this IServiceCollection services, Action<DbContextOptionsBuilder>? optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        {
            return services.AddDreamNumbersDbContext<DreamNumbersDbContext>(optionsAction, contextLifetime, optionsLifetime);
        }

        public static IServiceCollection AddAzureSqlDreamNumbersStorage(this IServiceCollection services, string connectionString)
        {
            return services.AddAzureSqlDreamNumbersStorage(options => options.UseAzureSql(connectionString));
        }

        public static IServiceCollection AddAzureSqlDreamNumbersStorage(this IServiceCollection services, Action<DbContextOptionsBuilder>? optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        {
            return services.AddDreamNumbersStorage<DreamNumbersDbContext>(optionsAction, contextLifetime, optionsLifetime);
        }
    }
}
