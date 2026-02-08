using DreamNumbers.Enums;
using DreamNumbers.Models;
using DreamNumbers.Strategies;

namespace DreamNumbers.Services
{
    public sealed class StrategyBuilder : IStrategyBuilder
    {
        private readonly IStrategyFactory _strategyFactory;

        public StrategyBuilder(IStrategyFactory strategyFactory)
        {
            _strategyFactory = strategyFactory;
        }

        public IScoringStrategy Build(SimulationProfile profile)
        {
            if (profile.StrategyType == StrategyType.Composite)
            {
                if (profile.CompositeParts == null || profile.CompositeParts.Count == 0)
                    throw new InvalidOperationException("Composite profile requires CompositeParts.");

                return _strategyFactory.Create(
                    StrategyType.Composite,
                    profile.Config,
                    profile.CompositeParts.Select(p => (p.Strategy, p.Weight))
                );
            }

            return _strategyFactory.Create(profile.StrategyType, profile.Config);
        }
    }
}
