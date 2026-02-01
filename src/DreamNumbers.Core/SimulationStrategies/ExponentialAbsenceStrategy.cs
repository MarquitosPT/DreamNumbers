using DreamNumbers.Models;
using DreamNumbers.Services;

namespace DreamNumbers.SimulationStrategies
{
    internal class ExponentialAbsenceStrategy : ISimulationStrategy
    {
        public string Name => "Exponential Absence";

        public double GetWeight(NumberStatistics s)
        {
            return Math.Pow(1.15, s.CurrentAbsence);
        }
    }
}
