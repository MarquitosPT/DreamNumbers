namespace DreamNumbers.Models
{
    public sealed class StrategyConfig
    {
        public int MaxMainNumber { get; init; } = 40;
        public int MaxDreamNumber { get; init; } = 5;

        public int WindowSize { get; init; } = 0;

        public bool NormalizeScores { get; init; } = true;

        public double Alpha { get; init; } = 0.05;
        public double FrequencySmoothing { get; init; } = 0.0;
        public double GapStabilityWeight { get; init; } = 1.0;
    }
}
