using DreamNumbers.Models;

namespace DreamNumbers.Services
{
    public interface ICombinationGenerationPresetService
    {
        event Action? OnPresetChanged;
        event Action? OnPresetUpdated;
        CombinationGenerationPreset GetActivePreset();
        void SetActivePreset(string name);
        void AddPreset(CombinationGenerationPreset preset);
        void RemovePreset(string name);
        void NotifyPresetUpdated();
        IReadOnlyList<CombinationGenerationPreset> Presets { get; }
    }
}
