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

                CombinationGenerationMode.SmartHybrid2 =>
                    GenerateSmartHybrid2(
                        mainScores,
                        dreamScores,
                        generationPreset.DefaultCombinationCount,
                        generationPreset.NumbersPerCombination,
                        generationPreset.HybridTopPercentage,
                        generationPreset.DiversityPenalty,
                        generationPreset.SimilarityAvoidance,
                        generationPreset.DreamTopBias),

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

        private static List<int> WeightedRandomSelection(
            Dictionary<int, double> scores,
            int amount,
            HashSet<int>? exclude = null)
        {
            var selected = new List<int>();
            var pool = scores
                .Where(kv => exclude == null || !exclude.Contains(kv.Key))
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            var rnd = new Random();

            amount = Math.Min(amount, pool.Count);

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

        private static List<SimulatedCombination> GenerateSmartHybrid2(
            Dictionary<int, double> mainScores,
            Dictionary<int, double> dreamScores,
            int count,
            int numbersPerCombination,
            double topPercentage,
            double diversityPenalty,
            double similarityAvoidance,
            double dreamTopBias)
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

            var globalUsage = orderedMain.ToDictionary(n => n, n => 0);

            for (int i = 0; i < count; i++)
            {
                var comboSet = new HashSet<int>();

                // 1) Núcleo forte com rotação
                int offset = i % Math.Max(1, topCount);
                var core = orderedMain.Skip(offset).Take(topCount).ToList();
                foreach (var n in core) comboSet.Add(n);

                // 2) Penalização de diversidade global
                var adjustedScores = mainScores.ToDictionary(
                    kv => kv.Key,
                    kv =>
                    {
                        double penalty = 1.0 / (1.0 + globalUsage[kv.Key] * diversityPenalty);
                        return kv.Value * penalty;
                    });

                // 3) Seleção probabilística
                var randoms = WeightedRandomSelection(adjustedScores, randomCount, exclude: comboSet);
                foreach (var n in randoms) comboSet.Add(n);

                // 4) Similaridade com combinações anteriores
                if (similarityAvoidance > 0 && combinations.Count > 0)
                {
                    double similarityScore = combinations
                        .Select(existing => existing.Numbers.Intersect(comboSet).Count())
                        .Average();

                    if (similarityScore > 0)
                    {
                        foreach (var n in comboSet.ToList())
                        {
                            adjustedScores[n] *= (1.0 - similarityAvoidance * (similarityScore / numbersPerCombination));
                        }
                    }
                }

                // 5) Atualizar uso global
                foreach (var n in comboSet) globalUsage[n]++;

                // 6) DreamNumber inteligente
                int dreamTopK = Math.Max(3, (int)(orderedDream.Count * 0.3));
                bool useTopDream = rnd.NextDouble() < dreamTopBias;

                int dream = useTopDream
                    ? orderedDream[rnd.Next(dreamTopK)]
                    : WeightedRandomSelection(dreamScores, 1).First();

                combinations.Add(new SimulatedCombination
                {
                    Numbers = comboSet.OrderBy(n => n).ToList(),
                    DreamNumber = dream
                });
            }

            return combinations;
        }
    }
}
