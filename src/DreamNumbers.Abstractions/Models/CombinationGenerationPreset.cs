using DreamNumbers.Enums;

namespace DreamNumbers.Models
{
    public sealed class CombinationGenerationPreset
    {
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }

        public CombinationGenerationMode Mode { get; init; }

        /// <summary>
        /// Percentagem de números vindos do Top-N (apenas para Hybrid).
        /// Ex: 0.7 significa 70% Top-N e 30% random.
        /// </summary>
        public double HybridTopPercentage { get; init; } = 0.7;

        /// <summary>
        /// Número de combinações a gerar por defeito.
        /// </summary>
        public int DefaultCombinationCount { get; init; } = 5;

        /// <summary>
        /// Quantos números por combinação.
        /// </summary>
        public int NumbersPerCombination { get; init; } = 6;

        public bool IsPreset { get; init; }
        public bool IsActive { get; set; }
    }
}
