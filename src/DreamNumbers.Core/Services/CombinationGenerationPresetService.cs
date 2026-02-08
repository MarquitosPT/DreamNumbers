using DreamNumbers.Enums;
using DreamNumbers.Models;

namespace DreamNumbers.Services
{
    public sealed class CombinationGenerationPresetService : ICombinationGenerationPresetService
    {
        private readonly List<CombinationGenerationPreset> _presets = [];

        public IReadOnlyList<CombinationGenerationPreset> Presets => _presets;

        public CombinationGenerationPresetService()
        {
            LoadPresets();
        }

        private void LoadPresets()
        {
            _presets.AddRange(
            [
                new CombinationGenerationPreset
                {
                    Name = "Deterministic Top-N",
                    Description = "Gera combinações usando sempre os números com maior score.",
                    Mode = CombinationGenerationMode.Deterministic,
                    DefaultCombinationCount = 5,
                    NumbersPerCombination = 6,
                    IsPreset = true,
                    IsActive = true
                },

                new CombinationGenerationPreset
                {
                    Name = "Probabilistic Weighted",
                    Description = "Gera combinações usando sorteio ponderado pelos scores.",
                    Mode = CombinationGenerationMode.Probabilistic,
                    DefaultCombinationCount = 5,
                    NumbersPerCombination = 6,
                    IsPreset = true
                },

                new CombinationGenerationPreset
                {
                    Name = "Hybrid Smart Mix",
                    Description = "Combina Top-N com randomização ponderada para maior diversidade.",
                    Mode = CombinationGenerationMode.Hybrid,
                    HybridTopPercentage = 0.7,
                    DefaultCombinationCount = 5,
                    NumbersPerCombination = 6,
                    IsPreset = true
                }
            ]);
        }

        public CombinationGenerationPreset GetActivePreset()
            => _presets.First(p => p.IsActive);

        public void SetActivePreset(string name)
        {
            foreach (var p in _presets)
                p.IsActive = p.Name == name;
        }

        public void AddPreset(CombinationGenerationPreset preset)
        {
            if (_presets.Any(p => p.Name == preset.Name))
                throw new InvalidOperationException($"Já existe um preset com o nome '{preset.Name}'.");

            //preset.IsPreset = false;
            _presets.Add(preset);
        }

        public void RemovePreset(string name)
        {
            var preset = _presets.FirstOrDefault(p => p.Name == name);

            if (preset == null)
                return;

            if (preset.IsPreset)
                throw new InvalidOperationException("Não é possível remover um preset.");

            _presets.Remove(preset);
        }
    }
}
