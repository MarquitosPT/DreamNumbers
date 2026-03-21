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
        public double HybridTopPercentage { get; set; } = 0.7;

        /// <summary>
        /// Penalização global para diversidade entre combinações (apenas para SmartHybrid2).
        /// </summary>
        public double DiversityPenalty { get; set; } = 0.5;

        /// <summary>
        /// Evitar combinações muito semelhantes (apenas para SmartHybrid2).
        /// </summary>
        public double SimilarityAvoidance { get; set; } = 0.4;

        /// <summary>
        /// Percentagem de vezes que escolhe do Top-K vs random (apenas para SmartHybrid2).
        /// </summary>
        public double DreamTopBias { get; set; } = 0.7;      

        /// <summary>
        /// Número de combinações a gerar por defeito.
        /// </summary>
        public int DefaultCombinationCount { get; init; } = 5;

        /// <summary>
        /// Quantos números por combinação.
        /// </summary>
        public int NumbersPerCombination { get; init; } = 6;

        /// <summary>
        /// Indica se este preset é um dos presets predefinidos (true)
        /// ou um preset personalizado criado pelo utilizador (false).
        /// </summary>
        public bool IsPreset { get; init; }

        /// <summary>
        /// Indica se este preset está atualmente ativo.
        /// Apenas um preset deve estar ativo de cada vez.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
