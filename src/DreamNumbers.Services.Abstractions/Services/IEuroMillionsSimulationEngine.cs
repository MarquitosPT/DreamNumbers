using DreamNumbers.Models;

namespace DreamNumbers.Services
{
    public interface IEuroMillionsSimulationEngine
    {
        EuroMillionsSimulationResult RunSimulation(IReadOnlyList<EuroMillionDraw> draws);
    }
}
