using DreamNumbers.Models;

namespace DreamNumbers.Services
{
    public interface ISimulationEngine
    {
        SimulationResult RunSimulation(IReadOnlyList<Draw> draws);
    }

}
