using DreamNumbers.Models;
using DreamNumbers.Services;
using DreamNumbers.Storages;
using Microsoft.AspNetCore.Components;

namespace DreamNumbers.UI.Components
{
    public partial class EuroMillionsSimulation : ComponentBase
    {
        private EuroMillionsSimulationResult? result;
        private IReadOnlyList<EuroMillionDraw> draws = [];

        [Inject] public IEuroMillionDrawStorage DrawRepository { get; set; } = default!;
        [Inject] IEuroMillionsSimulationEngine Engine { get; set; } = null!;

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
