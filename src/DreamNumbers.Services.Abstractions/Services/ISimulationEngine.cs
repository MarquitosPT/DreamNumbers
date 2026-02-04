using DreamNumbers.Models;

namespace DreamNumbers.Services
{
    public interface ISimulationEngine
    {
        SimulationResult Generate(
            SimulationRequest request,
            List<NumberStatistics> stats,
            List<DreamNumberStatistics> dreamStats,
            ISimulationStrategy strategy);
    }

}
