using DreamNumbers.Models;
using DreamNumbers.Strategies;

namespace DreamNumbers.Services
{
    public interface IStrategyBuilder
    {
        IScoringStrategy Build(SimulationProfile profile);
    }
}
