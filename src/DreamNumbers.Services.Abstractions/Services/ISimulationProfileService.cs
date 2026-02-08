using DreamNumbers.Models;

namespace DreamNumbers.Services
{
    public interface ISimulationProfileService
    {
        SimulationProfile GetActiveProfile();
        void SetActiveProfile(string name);
        void AddProfile(SimulationProfile profile);
        void RemoveProfile(string name);
        void UpdateProfile(SimulationProfile profile);

        IReadOnlyList<SimulationProfile> Profiles { get; }

        //public IScoringStrategy BuildActiveStrategy();
    }
}
