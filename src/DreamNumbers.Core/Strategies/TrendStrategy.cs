using DreamNumbers.Models;

namespace DreamNumbers.Strategies
{
    public sealed class TrendStrategy : IScoringStrategy
    {
        public string Name => "Trend";

        public Dictionary<int, double> CalculateMainNumberScores(
            IReadOnlyList<Draw> draws,
            IReadOnlyList<NumberStatistics> stats,
            StrategyConfig config)
        {
            return CalculateMainTrend(draws, stats, config);
        }

        public Dictionary<int, double> CalculateDreamNumberScores(
            IReadOnlyList<Draw> draws,
            IReadOnlyList<DreamNumberStatistics> stats,
            StrategyConfig config)
        {
            return CalculateDreamTrend(draws, stats, config);
        }

        private static Dictionary<int, double> CalculateMainTrend(
            IReadOnlyList<Draw> draws,
            IReadOnlyList<NumberStatistics> stats,
            StrategyConfig config)
        {
            int window = config.WindowSize > 0 ? config.WindowSize : draws.Count / 2;
            int split = Math.Max(1, draws.Count - window);

            var recent = draws.Skip(split).ToList();
            var old = draws.Take(split).ToList();

            var scores = new Dictionary<int, double>();

            foreach (var s in stats)
            {
                double recentCount = recent.Count(d => d.Numbers.Contains(s.Number));
                double oldCount = old.Count(d => d.Numbers.Contains(s.Number));

                double trend = recentCount - oldCount;

                scores[s.Number] = trend;
            }

            return Normalize(scores);
        }

        private static Dictionary<int, double> CalculateDreamTrend(
            IReadOnlyList<Draw> draws,
            IReadOnlyList<DreamNumberStatistics> stats,
            StrategyConfig config)
        {
            int window = config.WindowSize > 0 ? config.WindowSize : draws.Count / 2;
            int split = Math.Max(1, draws.Count - window);

            var recent = draws.Skip(split).ToList();
            var old = draws.Take(split).ToList();

            var scores = new Dictionary<int, double>();

            foreach (var s in stats)
            {
                double recentCount = recent.Count(d => d.DreamNumber == s.Number);
                double oldCount = old.Count(d => d.DreamNumber == s.Number);

                double trend = recentCount - oldCount;

                scores[s.Number] = trend;
            }

            return Normalize(scores);
        }

        private static Dictionary<int, double> Normalize(Dictionary<int, double> scores)
        {
            double max = scores.Values.Max();
            double min = scores.Values.Min();

            if (max == min)
                return scores.ToDictionary(k => k.Key, _ => 0.0);

            return scores.ToDictionary(
                k => k.Key,
                v => (v.Value - min) / (max - min));
        }
    }
}
