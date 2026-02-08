using DreamNumbers.Enums;
using DreamNumbers.Models;
using DreamNumbers.Strategies;

namespace DreamNumbers.Services
{
    public class StrategyFactory : IStrategyFactory
    {
        public IScoringStrategy Create(
            StrategyType type,
            StrategyConfig config,
            IEnumerable<(StrategyType Strategy, double Weight)>? compositeParts = null)
        {
            return type switch
            {
                StrategyType.MedianGap => new MedianGapStrategy(),
                StrategyType.HazardRate => new HazardRateStrategy(),
                StrategyType.Trend => new TrendStrategy(),
                StrategyType.FrequencyScore => new FrequencyScoreStrategy(),
                StrategyType.RecencyDecay => new RecencyDecayStrategy(),
                StrategyType.GapStability => new GapStabilityStrategy(),

                StrategyType.Composite => CreateComposite(config, compositeParts),

                _ => throw new NotImplementedException($"Strategy '{type}' not implemented.")
            };
        }

        private CompositeStrategy CreateComposite(
            StrategyConfig config,
            IEnumerable<(StrategyType Strategy, double Weight)>? parts)
        {
            if (parts == null || !parts.Any())
                throw new ArgumentException("Composite strategy requires at least one sub-strategy.");

            var strategies = parts.Select(p =>
            {
                var strategy = Create(p.Strategy, config);
                return (strategy, p.Weight);
            });

            return new CompositeStrategy(strategies);
        }
    }
}
