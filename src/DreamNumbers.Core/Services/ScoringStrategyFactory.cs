using DreamNumbers.Enums;
using DreamNumbers.ScoringStrategies;

namespace DreamNumbers.Services;

internal class ScoringStrategyFactory : IScoringStrategyFactory
{
    public IScoringStrategy CreateProfile(
        ScoringStrategy strategy,
        SimulationProfile profile,
        params double[] weights)
    {
        // Estratégias simples (não compostas)
        if (strategy != ScoringStrategy.Composite)
        {
            return strategy switch
            {
                ScoringStrategy.MedianGap => new MedianGapStrategy(),
                ScoringStrategy.HazardRate => new HazardRateStrategy(),
                ScoringStrategy.Trend => new TrendStrategy(),
                _ => throw new NotImplementedException()
            };
        }

        // Composite + perfil predefinido
        if (profile != SimulationProfile.Personalizado)
        {
            return profile switch
            {
                SimulationProfile.Conservador => new CompositeStrategy(
                    (new TrendStrategy(), 0.6),
                    (new HazardRateStrategy(), 0.3),
                    (new MedianGapStrategy(), 0.1)
                ),

                SimulationProfile.Equilibrado => new CompositeStrategy(
                    (new TrendStrategy(), 0.33),
                    (new HazardRateStrategy(), 0.33),
                    (new MedianGapStrategy(), 0.33)
                ),

                SimulationProfile.Agressivo => new CompositeStrategy(
                    (new MedianGapStrategy(), 0.6),
                    (new HazardRateStrategy(), 0.3),
                    (new TrendStrategy(), 0.1)
                ),

                _ => throw new NotImplementedException()
            };
        }

        // Composite + perfil personalizado (sliders)
        if (weights.Length != 3)
            throw new ArgumentException("Personalized profile requires 3 weights.");

        return new CompositeStrategy(
            (new MedianGapStrategy(), weights[0]),
            (new HazardRateStrategy(), weights[1]),
            (new TrendStrategy(), weights[2])
        );
    }
}
