using DreamNumbers.Models;

namespace DreamNumbers.Strategies
{
    public sealed class MedianGapStrategy : IScoringStrategy
    {
        public string Name => "MedianGap";

        public Dictionary<int, double> CalculateMainNumberScores(
            IReadOnlyList<Draw> draws,
            IReadOnlyList<NumberStatistics> stats,
            StrategyConfig config)
            => CalculateScores(stats);

        public Dictionary<int, double> CalculateDreamNumberScores(
            IReadOnlyList<Draw> draws,
            IReadOnlyList<DreamNumberStatistics> stats,
            StrategyConfig config)
            => CalculateScores(stats);

        private static Dictionary<int, double> CalculateScores<T>(IReadOnlyList<T> stats)
            where T : BaseNumberStatistics
        {
            var scores = new Dictionary<int, double>();

            foreach (var s in stats)
            {
                if (s.Gaps.Count == 0)
                {
                    scores[s.Number] = 0;
                    continue;
                }

                var sorted = s.Gaps.OrderBy(x => x).ToList();
                double median = sorted[sorted.Count / 2];

                double diff = Math.Abs(s.Gap - median);

                scores[s.Number] = 1.0 / (1.0 + diff);
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
