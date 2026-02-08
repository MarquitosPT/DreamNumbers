using DreamNumbers.Models;
using DreamNumbers.Services;
using DreamNumbers.Storages;
using Microsoft.AspNetCore.Components;

namespace DreamNumbers.UI.Pages
{
    public partial class Simulation : ComponentBase
    {
        private SimulationResult? result;

        [Inject] public IDrawStorage DrawRepository { get; set; } = default!;
        [Inject] ISimulationEngine Engine { get; set; } = null!;

        private async Task RunSimulation()
        {
            var draws = await DrawRepository.GetAllAsync();

            result = Engine.RunSimulation(draws);
        }
    }
}
