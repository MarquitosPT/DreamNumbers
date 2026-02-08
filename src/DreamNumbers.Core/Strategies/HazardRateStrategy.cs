using DreamNumbers.Models;

namespace DreamNumbers.Strategies
{
    public sealed class HazardRateStrategy : IScoringStrategy
    {
        public string Name => "HazardRate";

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
                if (!s.HazardRates.TryGetValue(s.Gap, out double hazard))
                    hazard = 0;

                scores[s.Number] = hazard;
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
