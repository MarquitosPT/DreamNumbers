using DreamNumbers.Enums;

namespace DreamNumbers.Models
{
    public sealed class SimulationProfile
    {
        /// <summary>
        /// Nome do perfil (ex.: "Balanced Classic", "Recency Boost").
        /// </summary>
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// Descrição opcional do perfil.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Tipo de estratégia principal (pode ser Composite).
        /// </summary>
        public StrategyType StrategyType { get; init; }

        /// <summary>
        /// Lista de estratégias e respetivos pesos (apenas usada quando StrategyType = Composite).
        /// </summary>
        public List<StrategyWeight>? CompositeParts { get; init; }

        /// <summary>
        /// Configuração usada por todas as estratégias deste perfil.
        /// </summary>
        public StrategyConfig Config { get; init; } = new();

        /// <summary>
        /// Indica se este perfil é um preset do sistema.
        /// </summary>
        public bool IsPreset { get; init; }

        /// <summary>
        /// Indica se este perfil está ativo/selecionado.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
