using DreamNumbers.ScheduledTasks;
using DreamNumbers.Services;
using DreamNumbers.Services.EUDreams.Extensions.Configuration;
using DreamNumbers.Strategies;
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
                var nextMinute = DateTime.Now.AddMinutes(2).Minute;

                o.Schedule = Cron.Hourly(nextMinute);
                o.IsEnabled = true;
            });

            // Application services
            services.AddScoped<IDrawUpdateService, DrawUpdateService>();
            services.AddScoped<IStatisticsService, StatisticsService>();
            services.AddScoped<ICombinationGenerationPresetService, CombinationGenerationPresetService>();
            services.AddScoped<ISimulationProfileService, SimulationProfileService>();
            services.AddScoped<ISimulationEngine, SimulationEngine>();

            // Buliders e Factories
            services.AddSingleton<IStrategyBuilder, StrategyBuilder>();
            services.AddSingleton<IStrategyFactory, StrategyFactory>();

            // Strategies
            services.AddScoped<IScoringStrategy, TrendStrategy>();
            services.AddScoped<IScoringStrategy, HazardRateStrategy>();
            services.AddScoped<IScoringStrategy, MedianGapStrategy>();
            services.AddScoped<IScoringStrategy, FrequencyScoreStrategy>();
            services.AddScoped<IScoringStrategy, RecencyDecayStrategy>();
            services.AddScoped<IScoringStrategy, GapStabilityStrategy>();
            services.AddScoped<IScoringStrategy, CompositeStrategy>();

            services.AddEUDreamsService();

            return services;
        }
    }
}
