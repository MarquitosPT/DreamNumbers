using DreamNumbers.Models;
using DreamNumbers.Services;

namespace DreamNumbers.SimulationStrategies
{
    internal class AbsenceStrategy : ISimulationStrategy
    {
        public string Name => "Absence Only";

        public double GetWeight(NumberStatistics s)
        {
            return s.CurrentAbsence;
        }

        public double GetWeight(DreamNumberStatistics s)
        {
            return s.CurrentAbsence;
        }
    }
}
