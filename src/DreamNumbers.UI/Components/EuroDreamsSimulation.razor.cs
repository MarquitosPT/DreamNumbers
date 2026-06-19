using DreamNumbers.Models;
using DreamNumbers.Services;
using DreamNumbers.Storages;
using Microsoft.AspNetCore.Components;

namespace DreamNumbers.UI.Components
{
    public partial class EuroDreamsSimulation : ComponentBase
    {
        private SimulationResult? result;
        private IReadOnlyList<Draw> draws = [];

        [Inject] public IDrawStorage DrawRepository { get; set; } = default!;
        [Inject] ISimulationEngine Engine { get; set; } = null!;

        override protected async Task OnInitializedAsync()
        {
            draws = await DrawRepository.GetAllAsync();
        }

        private async Task RunSimulation()
        {
            result = Engine.RunSimulation(draws);
        }
    }
}
