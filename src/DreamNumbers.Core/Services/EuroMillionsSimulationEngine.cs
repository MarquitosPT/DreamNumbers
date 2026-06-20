using DreamNumbers.Enums;
using DreamNumbers.Models;

namespace DreamNumbers.Services
{
    internal class EuroMillionsSimulationEngine(IStatisticsService statisticsService,
        ISimulationProfileService profileService,
        IStrategyBuilder strategyBuilder,
        ICombinationGenerationPresetService generationPresetService) : IEuroMillionsSimulationEngine
    {
        private const int NumbersPerCombination = 5;
        private const int StarsPerCombination = 2;

        private readonly IStatisticsService _statisticsService = statisticsService;
        private readonly ISimulationProfileService _profileService = profileService;
        private readonly IStrategyBuilder _strategyBuilder = strategyBuilder;
        private readonly ICombinationGenerationPresetService _generationPresetService = generationPresetService;

        public EuroMillionsSimulationResult RunSimulation(IReadOnlyList<EuroMillionDraw> draws)
        {
            if (draws.Count == 0)
                throw new InvalidOperationException("Não existem sorteios para simular.");

            // 1. Obter perfil ativo
            var profile = _profileService.GetActiveProfile();
            var generationPreset = _generationPresetService.GetActivePreset();

            // 2. Construir estratégia
            var strategy = _strategyBuilder.Build(profile);

            // 3. Calcular estatísticas
            var numberStats = _statisticsService.CalculateNumberStatistics(draws, profile.Config.MaxMainNumber);
            var starStats = _statisticsService.CalculateStarStatistics(draws, profile.Config.MaxDreamNumber);

            // 4. Calcular scores
            var numberScores = strategy.CalculateNumberScores(draws, numberStats, profile.Config);
            var starScores = strategy.CalculateStarScores(draws, starStats, profile.Config);

            // 5. Gerar combinações
            var combinations = generationPreset.Mode switch
            {
                CombinationGenerationMode.Deterministic =>
                    GenerateDeterministic(numberScores, starScores,
                        generationPreset.DefaultCombinationCount),

                CombinationGenerationMode.Probabilistic =>
                    GenerateProbabilistic(numberScores, starScores,
                        generationPreset.DefaultCombinationCount),

                CombinationGenerationMode.Hybrid =>
                    GenerateHybrid(numberScores, starScores,
                        generationPreset.DefaultCombinationCount,
                        generationPreset.HybridTopPercentage),

                CombinationGenerationMode.SmartHybrid2 =>
                    GenerateSmartHybrid2(numberScores, starScores,
                        generationPreset.DefaultCombinationCount,
                        generationPreset.HybridTopPercentage,
                        generationPreset.DiversityPenalty,
                        generationPreset.SimilarityAvoidance,
                        generationPreset.DreamTopBias),

                _ => throw new NotImplementedException()
            };

            return new EuroMillionsSimulationResult
            {
                Combinations = combinations,
                NumberScores = numberScores,
                StarScores = starScores
            };
        }

        private static List<EuroMillionsSimulatedCombination> GenerateDeterministic(
            Dictionary<int, double> mainScores,
            Dictionary<int, double> starScores,
            int count)
        {
            var orderedMain = mainScores.OrderByDescending(kv => kv.Value).Select(kv => kv.Key).ToList();
            var orderedStar = starScores.OrderByDescending(kv => kv.Value).Select(kv => kv.Key).ToList();

            var combinations = new List<EuroMillionsSimulatedCombination>();

            for (int i = 0; i < count; i++)
            {
                combinations.Add(new EuroMillionsSimulatedCombination
                {
                    Numbers = [.. orderedMain.Skip(i).Take(NumbersPerCombination).OrderBy(n => n)],
                    Stars = [.. orderedStar.Skip(i).Take(StarsPerCombination).OrderBy(n => n)]
                });
            }

            return combinations;
        }

        private static List<EuroMillionsSimulatedCombination> GenerateProbabilistic(
            Dictionary<int, double> mainScores,
            Dictionary<int, double> starScores,
            int count)
        {
            var combinations = new List<EuroMillionsSimulatedCombination>();

            for (int i = 0; i < count; i++)
            {
                var numbers = WeightedRandomSelection(mainScores, NumbersPerCombination);
                var stars = WeightedRandomSelection(starScores, StarsPerCombination);

                combinations.Add(new EuroMillionsSimulatedCombination
                {
                    Numbers = [.. numbers.OrderBy(n => n)],
                    Stars = [.. stars.OrderBy(n => n)]
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

            return [.. selected.OrderBy(n => n)];
        }

        private static List<EuroMillionsSimulatedCombination> GenerateHybrid(
            Dictionary<int, double> numberScores,
            Dictionary<int, double> starScores,
            int count,
            double topPercentage)
        {
            var orderedNumbers = numberScores
                .OrderByDescending(kv => kv.Value)
                .Select(kv => kv.Key)
                .ToList();

            var orderedStars = starScores
                .OrderByDescending(kv => kv.Value)
                .Select(kv => kv.Key)
                .ToList();

            var combinations = new List<EuroMillionsSimulatedCombination>();

            int topCount = (int)Math.Round(NumbersPerCombination * topPercentage);
            int randomCount = NumbersPerCombination - topCount;

            for (int i = 0; i < count; i++)
            {
                var combo = new List<int>();

                // Parte determinística (Top-N)
                combo.AddRange(orderedNumbers.Take(topCount));

                // Parte probabilística
                var randoms = WeightedRandomSelection(numberScores, randomCount);
                combo.AddRange(randoms);

                // Seleção de estrelas
                var stars = WeightedRandomSelection(starScores, StarsPerCombination);

                combinations.Add(new EuroMillionsSimulatedCombination
                {
                    Numbers = [.. combo.OrderBy(n => n)],
                    Stars = [.. stars.OrderBy(n => n)]
                });
            }

            return combinations;
        }

        private static List<EuroMillionsSimulatedCombination> GenerateSmartHybrid2(
            Dictionary<int, double> numberScores,
            Dictionary<int, double> starScores,
            int count,
            double topPercentage,
            double diversityPenalty,
            double similarityAvoidance,
            double dreamTopBias)
        {
            var orderedNumbers = numberScores
                .OrderByDescending(kv => kv.Value)
                .Select(kv => kv.Key)
                .ToList();

            var orderedStars = starScores
                .OrderByDescending(kv => kv.Value)
                .Select(kv => kv.Key)
                .ToList();

            var combinations = new List<EuroMillionsSimulatedCombination>();
            var rnd = new Random();

            int topCount = (int)Math.Round(NumbersPerCombination * topPercentage);
            int randomCount = NumbersPerCombination - topCount;

            var globalUsage = orderedNumbers.ToDictionary(n => n, n => 0);

            for (int i = 0; i < count; i++)
            {
                var comboSet = new HashSet<int>();

                // 1) Núcleo forte com rotação
                int offset = i % Math.Max(1, topCount);
                var core = orderedNumbers.Skip(offset).Take(topCount).ToList();
                foreach (var n in core) comboSet.Add(n);

                // 2) Penalização de diversidade global
                var adjustedScores = numberScores.ToDictionary(
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
                            adjustedScores[n] *= (1.0 - similarityAvoidance * (similarityScore / NumbersPerCombination));
                        }
                    }
                }

                // 5) Atualizar uso global
                foreach (var n in comboSet) globalUsage[n]++;

                // 6) Estrelas com viés para as mais pontuadas
                var stars = WeightedRandomSelection(starScores, StarsPerCombination);

                combinations.Add(new EuroMillionsSimulatedCombination
                {
                    Numbers = [.. comboSet.OrderBy(n => n)],
                    Stars = [.. stars.OrderBy(n => n)]
                });
            }

            return combinations;
        }
    }
}
