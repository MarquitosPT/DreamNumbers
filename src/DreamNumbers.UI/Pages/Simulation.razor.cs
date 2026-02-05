using DreamNumbers.Enums;
using DreamNumbers.Models;
using DreamNumbers.Services;
using DreamNumbers.Storages;
using Microsoft.AspNetCore.Components;

namespace DreamNumbers.UI.Pages
{
    public partial class Simulation : ComponentBase
    {
        [Inject] public ISimulationEngine Engine { get; set; } = default!;
        [Inject] public IStatisticsService StatisticsService { get; set; } = default!;
        [Inject] public IDrawStorage DrawRepository { get; set; } = default!;

        [Inject] public IScoringStrategyFactory StrategyFactory { get; set; } = default!;

        protected string SelectedStrategyName = "Composite";
        protected string SelectedProfile = "Equilibrado";
        protected int Interval = 40;
        protected int Combinations = 5;

        protected double WeightMedianGap = 0.33;
        protected double WeightHazard = 0.33;
        protected double WeightTrend = 0.33;

        protected SimulationResult? Result;
        protected List<string> Logs = [];
        protected int? ExpandedLogIndex = null;

        protected async Task RunSimulation()
        {
            var draws = await DrawRepository.GetAllAsync();
            var mainStats = StatisticsService.CalculateStatistics(draws);
            var dreamStats = StatisticsService.CalculateDreamNumberStatistics(draws);

            var strategyEnum = Enum.Parse<ScoringStrategy>(SelectedStrategyName);
            var profileEnum = Enum.Parse<SimulationProfile>(SelectedProfile);

            IScoringStrategy strategy;

            if (profileEnum == SimulationProfile.Personalizado)
            {
                strategy = StrategyFactory.CreateProfile(
                    strategyEnum,
                    profileEnum,
                    WeightMedianGap,
                    WeightHazard,
                    WeightTrend
                );
            }
            else
            {
                strategy = StrategyFactory.CreateProfile(
                    strategyEnum,
                    profileEnum
                );
            }

            Result = Engine.Generate(
                Interval,
                Combinations,
                strategy,
                draws,
                mainStats,
                dreamStats
            );

            Logs = SimulationLogger.GenerateLogs(Result, strategy.Name, SelectedProfile, Interval);
        }

        protected void ToggleLog(int index)
        {
            ExpandedLogIndex = ExpandedLogIndex == index ? null : index;
        }
    }
}
