using DreamNumbers.Enums;
using DreamNumbers.Models;

namespace DreamNumbers.Services
{
    public sealed class SimulationEngine : ISimulationEngine
    {
        private readonly IStatisticsService _statisticsService;
        private readonly ISimulationProfileService _profileService;
        private readonly IStrategyBuilder _strategyBuilder;
        private readonly ICombinationGenerationPresetService _generationPresetService;

        public SimulationEngine(
            IStatisticsService statisticsService,
            ISimulationProfileService profileService,
            IStrategyBuilder strategyBuilder,
            ICombinationGenerationPresetService generationPresetService)
        {
            _statisticsService = statisticsService;
            _profileService = profileService;
            _strategyBuilder = strategyBuilder;
            _generationPresetService = generationPresetService;
        }

        public SimulationResult RunSimulation(
            IReadOnlyList<Draw> draws,
            int numberOfCombinations = 5,
            int numbersPerCombination = 6)
        {
            if (draws.Count == 0)
                throw new InvalidOperationException("Não existem sorteios para simular.");

            // 1. Obter perfil ativo
            var profile = _profileService.GetActiveProfile();
            var generationPreset = _generationPresetService.GetActivePreset();


            // 2. Construir estratégia
            var strategy = _strategyBuilder.Build(profile);

            // 3. Calcular estatísticas
            var mainStats = _statisticsService.CalculateMainNumberStatistics(draws, profile.Config.MaxMainNumber);
            var dreamStats = _statisticsService.CalculateDreamNumberStatistics(draws, profile.Config.MaxDreamNumber);

            // 4. Calcular scores
            var mainScores = strategy.CalculateMainNumberScores(draws, mainStats, profile.Config);
            var dreamScores = strategy.CalculateDreamNumberScores(draws, dreamStats, profile.Config);

            // 5. Gerar combinações
            var combinations = generationPreset.Mode switch
            {
                CombinationGenerationMode.Deterministic =>
                    GenerateDeterministic(mainScores, dreamScores,
                        generationPreset.DefaultCombinationCount,
                        generationPreset.NumbersPerCombination),

                CombinationGenerationMode.Probabilistic =>
                    GenerateProbabilistic(mainScores, dreamScores,
                        generationPreset.DefaultCombinationCount,
                        generationPreset.NumbersPerCombination),

                CombinationGenerationMode.Hybrid =>
                    GenerateHybrid(mainScores, dreamScores,
                        generationPreset.DefaultCombinationCount,
                        generationPreset.NumbersPerCombination,
                        generationPreset.HybridTopPercentage),

                _ => throw new NotImplementedException()
            };

            return new SimulationResult
            {
                Combinations = combinations,
                MainScores = mainScores,
                DreamScores = dreamScores
            };
        }

        private static List<SimulatedCombination> GenerateDeterministic(
            Dictionary<int, double> mainScores,
            Dictionary<int, double> dreamScores,
            int count,
            int numbersPerCombination)
        {
            var orderedMain = mainScores.OrderByDescending(kv => kv.Value).Select(kv => kv.Key).ToList();
            var orderedDream = dreamScores.OrderByDescending(kv => kv.Value).Select(kv => kv.Key).ToList();

            var combinations = new List<SimulatedCombination>();

            for (int i = 0; i < count; i++)
            {
                combinations.Add(new SimulatedCombination
                {
                    Numbers = orderedMain.Skip(i).Take(numbersPerCombination).OrderBy(n => n).ToList(),
                    DreamNumber = orderedDream[i % orderedDream.Count]
                });
            }

            return combinations;
        }

        private static List<SimulatedCombination> GenerateProbabilistic(
            Dictionary<int, double> mainScores,
            Dictionary<int, double> dreamScores,
            int count,
            int numbersPerCombination)
        {
            var combinations = new List<SimulatedCombination>();

            for (int i = 0; i < count; i++)
            {
                var numbers = WeightedRandomSelection(mainScores, numbersPerCombination);
                var dream = WeightedRandomSelection(dreamScores, 1).First();

                combinations.Add(new SimulatedCombination
                {
                    Numbers = numbers.OrderBy(n => n).ToList(),
                    DreamNumber = dream
                });
            }

            return combinations;
        }

        private static List<int> WeightedRandomSelection(Dictionary<int, double> scores, int amount)
        {
            var selected = new List<int>();
            var pool = new Dictionary<int, double>(scores);

            var rnd = new Random();

            for (int i = 0; i < amount; i++)
            {
                double total = pool.Values.Sum();
                double roll = rnd.NextDouble() * total;

                double cumulative = 0;
                int chosen = pool.First().Key;

                foreach (var kv in pool)
                {
                    cumulative += kv.Value;
                    if (roll <= cumulative)
                    {
                        chosen = kv.Key;
                        break;
                    }
                }

                selected.Add(chosen);
                pool.Remove(chosen);
            }

            return selected;
        }

        private static List<SimulatedCombination> GenerateHybrid(
            Dictionary<int, double> mainScores,
            Dictionary<int, double> dreamScores,
            int count,
            int numbersPerCombination,
            double topPercentage)
        {
            var orderedMain = mainScores
                .OrderByDescending(kv => kv.Value)
                .Select(kv => kv.Key)
                .ToList();

            var orderedDream = dreamScores
                .OrderByDescending(kv => kv.Value)
                .Select(kv => kv.Key)
                .ToList();

            var combinations = new List<SimulatedCombination>();
            var rnd = new Random();

            int topCount = (int)Math.Round(numbersPerCombination * topPercentage);
            int randomCount = numbersPerCombination - topCount;

            for (int i = 0; i < count; i++)
            {
                var combo = new List<int>();

                // Parte determinística (Top-N)
                combo.AddRange(orderedMain.Take(topCount));

                // Parte probabilística
                var randoms = WeightedRandomSelection(mainScores, randomCount);
                combo.AddRange(randoms);

                combinations.Add(new SimulatedCombination
                {
                    Numbers = combo.OrderBy(n => n).ToList(),
                    DreamNumber = orderedDream[rnd.Next(orderedDream.Count)]
                });
            }

            return combinations;
        }
    }
}
