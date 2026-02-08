using DreamNumbers.Models;

namespace DreamNumbers.Strategies
{
    public sealed class FrequencyScoreStrategy : IScoringStrategy
    {
        public string Name => "FrequencyScore";

        public Dictionary<int, double> CalculateMainNumberScores(
            IReadOnlyList<Draw> draws,
            IReadOnlyList<NumberStatistics> stats,
            StrategyConfig config)
            => CalculateScores(stats, draws.Count, config);

        public Dictionary<int, double> CalculateDreamNumberScores(
            IReadOnlyList<Draw> draws,
            IReadOnlyList<DreamNumberStatistics> stats,
            StrategyConfig config)
            => CalculateScores(stats, draws.Count, config);

        private static Dictionary<int, double> CalculateScores<T>(
            IReadOnlyList<T> stats,
            int interval,
            StrategyConfig config)
            where T : BaseNumberStatistics
        {
            double smoothing = config.FrequencySmoothing;

            var scores = new Dictionary<int, double>();

            foreach (var s in stats)
            {
                double freq = (s.Count + smoothing) / (interval + smoothing * stats.Count);
                scores[s.Number] = freq;
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
