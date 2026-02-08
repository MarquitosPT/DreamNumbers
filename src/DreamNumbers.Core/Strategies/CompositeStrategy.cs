using DreamNumbers.Models;

namespace DreamNumbers.Strategies
{
    public sealed class CompositeStrategy : IScoringStrategy
    {
        public string Name => "Composite";

        private readonly List<(IScoringStrategy Strategy, double Weight)> _strategies;

        public CompositeStrategy()
        {
            _strategies = [];
        }

        public CompositeStrategy(IEnumerable<(IScoringStrategy Strategy, double Weight)> strategies)
        {
            _strategies = [.. strategies];
        }

        public Dictionary<int, double> CalculateMainNumberScores(
            IReadOnlyList<Draw> draws,
            IReadOnlyList<NumberStatistics> stats,
            StrategyConfig config)
        {
            return Combine(s => s.CalculateMainNumberScores(draws, stats, config));
        }

        public Dictionary<int, double> CalculateDreamNumberScores(
            IReadOnlyList<Draw> draws,
            IReadOnlyList<DreamNumberStatistics> stats,
            StrategyConfig config)
        {
            return Combine(s => s.CalculateDreamNumberScores(draws, stats, config));
        }

        private Dictionary<int, double> Combine(
            Func<IScoringStrategy, Dictionary<int, double>> selector)
        {
            var result = new Dictionary<int, double>();

            foreach (var (strategy, weight) in _strategies)
            {
                var scores = selector(strategy);

                foreach (var kv in scores)
                {
                    if (!result.ContainsKey(kv.Key))
                        result[kv.Key] = 0;

                    result[kv.Key] += kv.Value * weight;
                }
            }

            return Normalize(result);
        }

        private static Dictionary<int, double> Normalize(Dictionary<int, double> scores)
        {
            double max = scores.Values.Max();
            if (max == 0) return scores;

            return scores.ToDictionary(k => k.Key, v => v.Value / max);
        }
    }
}
