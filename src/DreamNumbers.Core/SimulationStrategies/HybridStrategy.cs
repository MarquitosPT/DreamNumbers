using DreamNumbers.Models;
using DreamNumbers.Services;

namespace DreamNumbers.SimulationStrategies
{
    internal class HybridStrategy : ISimulationStrategy
    {
        public string Name => "Hybrid";

        public double GetWeight(NumberStatistics s)
        {
            double absence = s.CurrentAbsence;
            double frequency = s.Frequency20 * 0.5 +
                               s.Frequency40 * 0.3 +
                               s.Frequency60 * 0.2;

            return (absence * 0.6) + (frequency * 0.4);
        }
    }
}
