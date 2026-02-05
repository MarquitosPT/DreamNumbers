using DreamNumbers.Enums;

namespace DreamNumbers.Services
{
    public interface IScoringStrategyFactory
    {
        IScoringStrategy CreateProfile(
            ScoringStrategy strategy,
            SimulationProfile profile,
            params double[] weights);
    }
}
