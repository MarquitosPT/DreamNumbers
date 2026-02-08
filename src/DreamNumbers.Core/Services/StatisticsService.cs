using DreamNumbers.Models;

namespace DreamNumbers.Services
{
    public sealed class StatisticsService : IStatisticsService
    {
        public List<NumberStatistics> CalculateMainNumberStatistics(IReadOnlyList<Draw> draws, int maxNumber)
        {
            var stats = InitializeStats<NumberStatistics>(maxNumber);
            ComputeOccurrences(draws, stats, d => d.Numbers);
            ComputeGaps(stats, draws.Count);
            ComputeHazardRates(stats);

            return stats;
        }

        public List<DreamNumberStatistics> CalculateDreamNumberStatistics(IReadOnlyList<Draw> draws, int maxDreamNumber)
        {
            var stats = InitializeStats<DreamNumberStatistics>(maxDreamNumber);
            ComputeOccurrences(draws, stats, d => [d.DreamNumber]);
            ComputeGaps(stats, draws.Count);
            ComputeHazardRates(stats);

            return stats;
        }

        public List<DashboardMainStatistics> CalculateDashboardMainStatistics(IReadOnlyList<Draw> draws, int maxNumber)
        {
            var stats = InitializeDashboardStats<DashboardMainStatistics>(maxNumber);
            ComputeDashboardStats(draws, stats, d => d.Numbers);
            return stats;
        }
        public List<DashboardDreamStatistics> CalculateDashboardDreamStatistics(IReadOnlyList<Draw> draws, int maxDreamNumber)
        {
            var stats = InitializeDashboardStats<DashboardDreamStatistics>(maxDreamNumber);
            ComputeDashboardStats(draws, stats, d => [d.DreamNumber]);
            return stats;
        }

        private static List<T> InitializeDashboardStats<T>(int maxNumber)
            where T : BaseDashboardStatistics, new()
        {
            var list = new List<T>();

            for (int n = 1; n <= maxNumber; n++)
            {
                list.Add(new T
                {
                    Number = n
                });
            }

            return list;
        }

        private static List<T> InitializeStats<T>(int maxNumber)
            where T : BaseNumberStatistics, new()
        {
            var list = new List<T>();

            for (int n = 1; n <= maxNumber; n++)
            {
                list.Add(new T
                {
                    Number = n,
                    Count = 0,
                    LastSeenIndex = -1
                });
            }

            return list;
        }

        private static void ComputeDashboardStats<T>(IReadOnlyList<Draw> draws, List<T> stats, Func<Draw, IEnumerable<int>> selector)
            where T : BaseDashboardStatistics, new()
        {
            var orderedDraws = draws.OrderByDescending(d => d.Date).ToList();

            var last20 = orderedDraws
                .Take(20)
                .ToList();

            var last40 = orderedDraws
                .Take(40)
                .ToList();

            var last60 = orderedDraws
                .Take(60)
                .ToList();

            // Frequências para os últimos 20 sorteios
            for (int i = 0; i < last20.Count; i++)
            {
                var draw = last20[i];

                foreach (var num in selector(draw))
                {
                    var s = stats[num - 1];

                    s.Frequency20++;
                }
            }

            // Frequências para os últimos 40 sorteios
            for (int i = 0; i < last40.Count; i++)
            {
                var draw = last40[i];

                foreach (var num in selector(draw))
                {
                    var s = stats[num - 1];

                    s.Frequency40++;
                }
            }

            // Frequências para os últimos 60 sorteios
            for (int i = 0; i < last60.Count; i++)
            {
                var draw = last60[i];

                foreach (var num in selector(draw))
                {
                    var s = stats[num - 1];

                    s.Frequency60++;
                }
            }

            // Ausências atuais
            for (int i = 0; i < orderedDraws.Count; i++)
            {
                var draw = orderedDraws[i];

                foreach (var num in selector(draw))
                {
                    var s = stats[num - 1];

                    // Se ainda não foi registrado o índice da última ocorrência, registra agora
                    if (s.CurrentAbsence == -1)
                    {
                        s.CurrentAbsence = i;
                    }
                }
            }

            // Calcula a probabilidade estimada com base nas frequências e ausências
            foreach (var s in stats)
            {
                // Weighted probability model:
                // Higher absence → higher probability
                // Frequencies also influence the weight
                double absenceWeight = s.CurrentAbsence * 1.0;
                double frequencyWeight =
                      (s.Frequency20 * 0.5)
                    + (s.Frequency40 * 0.3)
                    + (s.Frequency60 * 0.2);

                double score = absenceWeight + frequencyWeight;

                // Normalize to a probability between 0 and 1
                double maxPossible = (60 * 1.0) + (20 * 0.5 + 40 * 0.3 + 60 * 0.2);

                s.EstimatedProbability = score / maxPossible;
            }
        }

        private static void ComputeOccurrences<T>(IReadOnlyList<Draw> draws, List<T> stats, Func<Draw, IEnumerable<int>> selector)
            where T : BaseNumberStatistics
        {
            var orderedDraws = draws.OrderByDescending(d => d.Date).ToList();

            for (int i = 0; i < orderedDraws.Count; i++)
            {
                var draw = orderedDraws[i];

                foreach (var num in selector(draw))
                {
                    var s = stats[num - 1];

                    if (s.LastSeenIndex != -1)
                    {
                        int gap = i - s.LastSeenIndex;
                        s.Gaps.Add(gap);
                    }

                    s.LastSeenIndex = i;
                    s.Count++;
                }
            }
        }

        private static void ComputeGaps<T>(List<T> stats, int totalDraws) where T : BaseNumberStatistics
        {
            foreach (var s in stats)
            {
                if (s.LastSeenIndex == -1)
                    s.Gap = totalDraws; // nunca saiu
                else
                    s.Gap = totalDraws - 1 - s.LastSeenIndex;
            }
        }

        private static void ComputeHazardRates<T>(List<T> stats)
            where T : BaseNumberStatistics
        {
            foreach (var s in stats)
            {
                if (s.Gaps.Count == 0)
                    continue;

                var groups = s.Gaps
                    .GroupBy(g => g)
                    .ToDictionary(g => g.Key, g => g.ToList());

                foreach (var kv in groups)
                {
                    int gap = kv.Key;
                    int occurrences = kv.Value.Count;

                    // HazardRate = ocorrências / total de gaps iguais
                    double hazard = (double)occurrences / groups[gap].Count;

                    s.HazardRates[gap] = hazard;
                }
            }
        }
    }
}
