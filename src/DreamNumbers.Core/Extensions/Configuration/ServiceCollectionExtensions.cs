using DreamNumbers.ScheduledTasks;
using DreamNumbers.Services;
using Marquitos.Schedulers;
using Marquitos.Schedulers.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DreamNumbers.Extensions.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDreamNumbersCore(this IServiceCollection services)
        {
            // Register core services here in the future.
            services.AddHttpClient();

            // Scheduler
            services.AddSchedulerService();
            services.AddScheduledTask<DrawUpdateTask>(o =>
            {
                o.Schedule = Cron.Daily(2);
                o.IsEnabled = true;
            });

            // Application services
            //services.AddScoped<IStatisticsService, StatisticsService>();
            //services.AddScoped<ISimulationEngine, SimulationEngine>();
            services.AddScoped<IDrawUpdateService, DrawUpdateService>();

            // Strategies
            //services.AddSingleton<ISimulationStrategy, AbsenceStrategy>();
            //services.AddSingleton<ISimulationStrategy, FrequencyStrategy>();
            //services.AddSingleton<ISimulationStrategy, HybridStrategy>();
            //services.AddSingleton<ISimulationStrategy, ExponentialAbsenceStrategy>();

            return services;
        }
    }
}
