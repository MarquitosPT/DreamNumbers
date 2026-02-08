using DreamNumbers.Models;

namespace DreamNumbers.Services
{
    public interface ICombinationGenerationPresetService
    {
        CombinationGenerationPreset GetActivePreset();
        void SetActivePreset(string name);
        void AddPreset(CombinationGenerationPreset preset);
        void RemovePreset(string name);

        IReadOnlyList<CombinationGenerationPreset> Presets { get; }
    }
}
