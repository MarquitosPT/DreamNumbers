using DreamNumbers.Enums;

namespace DreamNumbers.Models
{
    public sealed class StrategyWeight
    {
        public StrategyType Strategy { get; init; }
        public double Weight { get; init; }
    }
}
