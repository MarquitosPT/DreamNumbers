using DreamNumbers.Models;

namespace DreamNumbers.Strategies
{
    public sealed class GapStabilityStrategy : IScoringStrategy
    {
        public string Name => "GapStability";

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
            double weight = config.GapStabilityWeight <= 0 ? 1.0 : config.GapStabilityWeight;

            var scores = new Dictionary<int, double>();

            foreach (var s in stats)
            {
                if (s.Gaps.Count < 2)
                {
                    scores[s.Number] = 0;
                    continue;
                }

                double mean = s.Gaps.Average();
                double variance = s.Gaps.Sum(g => Math.Pow(g - mean, 2)) / s.Gaps.Count;
                double std = Math.Sqrt(variance);

                double stability = 1.0 / (1.0 + std);

                scores[s.Number] = stability * weight;
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
