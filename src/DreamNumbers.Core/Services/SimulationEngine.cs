using DreamNumbers.Models;

namespace DreamNumbers.Services
{
    internal class SimulationEngine : ISimulationEngine
    {
        private readonly Random _random = new();

        public SimulationResult Generate(
            SimulationRequest request,
            List<NumberStatistics> stats,
            List<DreamNumberStatistics> dreamStats,
            ISimulationStrategy strategy)
        {
            var result = new SimulationResult();

            for (int i = 0; i < request.NumberOfCombinations; i++)
            {
                var combination = GenerateSingleCombination(stats, strategy);
                var dreamCombination = GenerateSingleCombination(dreamStats, strategy);

                result.Combinations.Add((combination, dreamCombination));
            }

            return result;
        }

        private List<int> GenerateSingleCombination(
            List<NumberStatistics> stats,
            ISimulationStrategy strategy)
        {
            var pool = BuildWeightedPool(stats, strategy);
            var combination = new HashSet<int>();

            while (combination.Count < 6)
            {
                int number = pool[_random.Next(pool.Count)];
                combination.Add(number);
            }

            return combination.OrderBy(n => n).ToList();
        }

        private int GenerateSingleCombination(
            List<DreamNumberStatistics> stats,
            ISimulationStrategy strategy)
        {
            var pool = BuildWeightedPool(stats, strategy);

            int number = pool[_random.Next(pool.Count)];

            return number;
        }

        private static List<int> BuildWeightedPool(
            List<NumberStatistics> stats,
            ISimulationStrategy strategy)
        {
            var pool = new List<int>();

            foreach (var s in stats)
            {
                double weight = strategy.GetWeight(s);

                int count = (int)Math.Max(1, weight);

                for (int i = 0; i < count; i++)
                    pool.Add(s.Number);
            }

            return pool;
        }

        private static List<int> BuildWeightedPool(
            List<DreamNumberStatistics> stats,
            ISimulationStrategy strategy)
        {
            var pool = new List<int>();

            foreach (var s in stats)
            {
                double weight = strategy.GetWeight(s);

                int count = (int)Math.Max(1, weight);

                for (int i = 0; i < count; i++)
                    pool.Add(s.Number);
            }

            return pool;
        }
    }
}
