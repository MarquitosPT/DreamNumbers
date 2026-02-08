using DreamNumbers.Enums;
using DreamNumbers.Models;

namespace DreamNumbers.Services
{
    public sealed class SimulationProfileService : ISimulationProfileService
    {
        private readonly List<SimulationProfile> _profiles = [];

        public IReadOnlyList<SimulationProfile> Profiles => _profiles;

        public SimulationProfileService()
        {
            LoadPresets();
        }

        // ------------------------------------------------------------
        // PRESETS
        // ------------------------------------------------------------
        private void LoadPresets()
        {
            _profiles.AddRange(
            [
                new SimulationProfile
                {
                    Name = "Balanced Classic",
                    Description = "Combinação equilibrada das estratégias clássicas.",
                    StrategyType = StrategyType.Composite,
                    CompositeParts =
                    [
                        new() { Strategy = StrategyType.MedianGap,    Weight = 0.4 },
                        new() { Strategy = StrategyType.HazardRate,   Weight = 0.3 },
                        new() { Strategy = StrategyType.Trend,        Weight = 0.3 }
                    ],
                    Config = new StrategyConfig(),
                    IsPreset = true,
                    IsActive = true
                },

                new SimulationProfile
                {
                    Name = "Recency Boost",
                    Description = "Favorece números que saíram recentemente.",
                    StrategyType = StrategyType.RecencyDecay,
                    Config = new StrategyConfig { Alpha = 0.08 },
                    IsPreset = true
                },

                new SimulationProfile
                {
                    Name = "Stability Mix",
                    Description = "Favorece números com comportamento estável.",
                    StrategyType = StrategyType.Composite,
                    CompositeParts =
                    [
                        new() { Strategy = StrategyType.GapStability,   Weight = 0.5 },
                        new() { Strategy = StrategyType.FrequencyScore, Weight = 0.3 },
                        new() { Strategy = StrategyType.Trend,          Weight = 0.2 }
                    ],
                    Config = new StrategyConfig { GapStabilityWeight = 1.2 },
                    IsPreset = true
                },

                new SimulationProfile
                {
                    Name = "Full Composite",
                    Description = "Todas as estratégias combinadas de forma equilibrada.",
                    StrategyType = StrategyType.Composite,
                    CompositeParts =
                    [
                        new() { Strategy = StrategyType.MedianGap,      Weight = 0.2 },
                        new() { Strategy = StrategyType.HazardRate,     Weight = 0.2 },
                        new() { Strategy = StrategyType.Trend,          Weight = 0.2 },
                        new() { Strategy = StrategyType.FrequencyScore, Weight = 0.2 },
                        new() { Strategy = StrategyType.RecencyDecay,   Weight = 0.1 },
                        new() { Strategy = StrategyType.GapStability,   Weight = 0.1 }
                    ],
                    Config = new StrategyConfig(),
                    IsPreset = true
                }
            ]);
        }

        // ------------------------------------------------------------
        // PERFIL ATIVO
        // ------------------------------------------------------------
        public SimulationProfile GetActiveProfile()
            => _profiles.First(p => p.IsActive);

        public void SetActiveProfile(string name)
        {
            foreach (var p in _profiles)
                p.IsActive = p.Name == name;
        }

        // ------------------------------------------------------------
        // CRUD DE PERFIS DO UTILIZADOR
        // ------------------------------------------------------------
        public void AddProfile(SimulationProfile profile)
        {
            if (_profiles.Any(p => p.Name == profile.Name))
                throw new InvalidOperationException($"Já existe um perfil com o nome '{profile.Name}'.");

            //profile.IsPreset = false;
            _profiles.Add(profile);
        }

        public void RemoveProfile(string name)
        {
            var profile = _profiles.FirstOrDefault(p => p.Name == name);

            if (profile == null)
                return;

            if (profile.IsPreset)
                throw new InvalidOperationException("Não é possível remover um preset.");

            _profiles.Remove(profile);
        }

        public void UpdateProfile(SimulationProfile updated)
        {
            var existing = _profiles.FirstOrDefault(p => p.Name == updated.Name);

            if (existing == null)
                throw new InvalidOperationException("Perfil não encontrado.");

            if (existing.IsPreset)
                throw new InvalidOperationException("Não é possível alterar um preset.");

            int index = _profiles.IndexOf(existing);
            _profiles[index] = updated;
        }
    }
}
