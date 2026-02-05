using DreamNumbers.ScheduledTasks;
using DreamNumbers.ScoringStrategies;
using DreamNumbers.Services;
using DreamNumbers.Services.EUDreams.Extensions.Configuration;
using Marquitos.Schedulers;
using Marquitos.Schedulers.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DreamNumbers.Extensions.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDreamNumbersCore(this IServiceCollection services)
        {
            
            // Scheduler
            services.AddSchedulerService();
            services.AddScheduledTask<DrawUpdateTask>(o =>
            {
                o.Schedule = Cron.Daily(2);
                o.IsEnabled = true;
            });

            // Application services
            services.AddScoped<IStatisticsService, StatisticsService>();
            services.AddScoped<ISimulationEngine, SimulationEngine>();
            services.AddScoped<IDrawUpdateService, DrawUpdateService>();

            // Factories
            services.AddSingleton<IScoringStrategyFactory, ScoringStrategyFactory>();

            // Strategies
            services.AddSingleton<IScoringStrategy, TrendStrategy>();
            services.AddSingleton<IScoringStrategy, HazardRateStrategy>();
            services.AddSingleton<IScoringStrategy, MedianGapStrategy>();
            services.AddSingleton<IScoringStrategy, CompositeStrategy>();

            services.AddEUDreamsService();

            return services;
        }
    }
}
