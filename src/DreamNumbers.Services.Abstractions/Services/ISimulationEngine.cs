using DreamNumbers.Models;

namespace DreamNumbers.Services
{
    public interface ISimulationEngine
    {
        SimulationResult Generate(
            int interval,
            int combinations,
            IScoringStrategy strategy,
            List<Draw> draws,
            List<NumberStatistics> mainStats,
            List<DreamNumberStatistics> dreamStats);
    }

}
