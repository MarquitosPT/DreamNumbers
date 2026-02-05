using DreamNumbers.Models;
using DreamNumbers.Services;

namespace DreamNumbers.ScoringStrategies
{
    internal class MedianGapStrategy : IScoringStrategy
    {
        public string Name => "Median Gap Strategy";

        public Dictionary<int, double> CalculateMainNumberScores(
            List<Draw> draws,
            List<NumberStatistics> stats,
            int interval)
        {
            return CalculateScores(draws, stats.Select(s => s.Number), d => d.Numbers);
        }

        public Dictionary<int, double> CalculateDreamNumberScores(
            List<Draw> draws,
            List<DreamNumberStatistics> stats,
            int interval)
        {
            return CalculateScores(draws, stats.Select(s => s.Number), d => new List<int> { d.DreamNumber });
        }

        private Dictionary<int, double> CalculateScores(
            List<Draw> draws,
            IEnumerable<int> universe,
            Func<Draw, List<int>> selector)
        {
            var scores = new Dictionary<int, double>();
            var ordered = draws.OrderBy(d => d.Date).ToList();

            foreach (var number in universe)
            {
                var appearances = ordered
                    .Select((d, idx) => new { d, idx })
                    .Where(x => selector(x.d).Contains(number))
                    .Select(x => x.idx)
                    .ToList();

                if (appearances.Count < 2)
                {
                    scores[number] = 0;
                    continue;
                }

                var gaps = new List<int>();
                for (int i = 1; i < appearances.Count; i++)
                    gaps.Add(appearances[i] - appearances[i - 1]);

                var median = gaps.OrderBy(x => x).ElementAt(gaps.Count / 2);

                int lastAppearance = appearances.Last();
                int currentGap = ordered.Count - lastAppearance - 1;

                scores[number] = (double)currentGap / median;
            }

            return scores;
        }
    }
}
