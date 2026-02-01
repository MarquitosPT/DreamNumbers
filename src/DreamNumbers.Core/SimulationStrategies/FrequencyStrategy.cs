using DreamNumbers.Models;
using DreamNumbers.Services;

namespace DreamNumbers.SimulationStrategies
{
    internal class FrequencyStrategy : ISimulationStrategy
    {
        public string Name => "Frequency Only";

        public double GetWeight(NumberStatistics s)
        {
            return s.Frequency20 * 0.5 +
                   s.Frequency40 * 0.3 +
                   s.Frequency60 * 0.2;
        }
    }
}
