using DreamNumbers.Models;

namespace DreamNumbers.Services
{
    public interface ISimulationStrategy
    {
        string Name { get; }
        double GetWeight(NumberStatistics stats);
    }

}
