using DreamNumbers.Models;

namespace DreamNumbers.Services
{
    public interface ISimulationEngine
    {
        SimulationResult RunSimulation(IReadOnlyList<Draw> draws, int numberOfCombinations = 5, int numbersPerCombination = 6);
    }

}
