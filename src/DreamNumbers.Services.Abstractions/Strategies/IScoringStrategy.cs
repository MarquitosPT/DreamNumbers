using DreamNumbers.Models;

namespace DreamNumbers.Strategies
{
    public interface IScoringStrategy
    {
        string Name { get; }

        Dictionary<int, double> CalculateMainNumberScores(
            IReadOnlyList<Draw> draws,
            IReadOnlyList<NumberStatistics> stats,
            StrategyConfig config);

        Dictionary<int, double> CalculateDreamNumberScores(
            IReadOnlyList<Draw> draws,
            IReadOnlyList<DreamNumberStatistics> stats,
            StrategyConfig config);

        Dictionary<int, double> CalculateNumberScores(
            IReadOnlyList<EuroMillionDraw> draws,
            IReadOnlyList<NumberStatistics> stats,
            StrategyConfig config);

        Dictionary<int, double> CalculateStarScores(
            IReadOnlyList<EuroMillionDraw> draws,
            IReadOnlyList<StarStatistics> stats,
            StrategyConfig config);
    }
}
