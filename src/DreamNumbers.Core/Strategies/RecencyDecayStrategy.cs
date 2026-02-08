using DreamNumbers.Models;

namespace DreamNumbers.Strategies
{
    public sealed class RecencyDecayStrategy : IScoringStrategy
    {
        public string Name => "RecencyDecay";

        public Dictionary<int, double> CalculateMainNumberScores(
            IReadOnlyList<Draw> draws,
            IReadOnlyList<NumberStatistics> stats,
            StrategyConfig config)
            => CalculateScores(stats, config);

        public Dictionary<int, double> CalculateDreamNumberScores(
            IReadOnlyList<Draw> draws,
            IReadOnlyList<DreamNumberStatistics> stats,
            StrategyConfig config)
            => CalculateScores(stats, config);

        private static Dictionary<int, double> CalculateScores<T>(
            IReadOnlyList<T> stats,
            StrategyConfig config)
            where T : BaseNumberStatistics
        {
            double alpha = config.Alpha <= 0 ? 0.05 : config.Alpha;

            var scores = new Dictionary<int, double>();

            foreach (var s in stats)
            {
                double decay = Math.Exp(-alpha * s.Gap);
                scores[s.Number] = decay;
            }

            return Normalize(scores);
        }

        private static Dictionary<int, double> Normalize(Dictionary<int, double> scores)
        {
            double max = scores.Values.Max();
            if (max == 0) return scores;

            return scores.ToDictionary(k => k.Key, v => v.Value / max);
        }
    }
}
