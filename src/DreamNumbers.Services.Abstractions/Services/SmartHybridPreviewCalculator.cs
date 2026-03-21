using DreamNumbers.Models;

namespace DreamNumbers.Services
{
    public static class SmartHybridPreviewCalculator
    {
        public static SmartHybridPreviewResult Calculate(
            Dictionary<int, double> mainScores,
            Dictionary<int, double> dreamScores,
            int numbersPerCombination,
            double topPercentage,
            double diversityPenalty,
            double similarityAvoidance,
            double dreamTopBias)
        {
            var orderedMain = mainScores.OrderByDescending(kv => kv.Value).Select(kv => kv.Key).ToList();
            var orderedDream = dreamScores.OrderByDescending(kv => kv.Value).Select(kv => kv.Key).ToList();

            int topCount = (int)Math.Round(numbersPerCombination * topPercentage);
            int randomCount = numbersPerCombination - topCount;

            var result = new SmartHybridPreviewResult
            {
                MainScores = mainScores,
                DreamScores = dreamScores
            };

            // Núcleo forte
            var core = orderedMain.Take(topCount).ToList();

            // Penalização global simulada
            result.GlobalPenalty = diversityPenalty * 0.5;

            // Similaridade simulada
            result.Similarity = similarityAvoidance * 0.4;

            // Randomização ponderada
            var rnd = new Random();
            var randoms = mainScores
                .OrderByDescending(kv => kv.Value * (1 - diversityPenalty))
                .Skip(topCount)
                .Take(randomCount)
                .Select(kv => kv.Key)
                .ToList();

            result.Numbers = core.Concat(randoms).OrderBy(n => n).ToList();

            // DreamNumber
            bool useTop = rnd.NextDouble() < dreamTopBias;
            result.DreamNumber = useTop
                ? orderedDream.First()
                : orderedDream[rnd.Next(orderedDream.Count)];

            result.AdjustedMainScores = CalculateAdjustedScores(
                mainScores,
                diversityPenalty,
                similarityAvoidance,
                numbersPerCombination
            );

            return result;
        }

        private static Dictionary<int, double> CalculateAdjustedScores(
            Dictionary<int, double> baseScores,
            double diversityPenalty,
            double similarityAvoidance,
            int numbersPerCombination)
        {
            var adjusted = new Dictionary<int, double>();

            foreach (var kv in baseScores)
            {
                double score = kv.Value;

                // Penalização global
                score *= (1.0 - diversityPenalty * 0.5);

                // Penalização de similaridade
                score *= (1.0 - similarityAvoidance * 0.4);

                adjusted[kv.Key] = Math.Max(score, 0.0001);
            }

            return adjusted;
        }
    }
}
