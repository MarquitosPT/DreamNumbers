using DreamNumbers.Enums;
using DreamNumbers.Models;
using DreamNumbers.Strategies;

namespace DreamNumbers.Services
{
    public interface IStrategyFactory
    {
        IScoringStrategy Create(StrategyType type, StrategyConfig config, IEnumerable<(StrategyType Strategy, double Weight)>? compositeParts = null);
    }
}
