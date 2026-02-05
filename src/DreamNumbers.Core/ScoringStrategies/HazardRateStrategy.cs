using DreamNumbers.Models;
using DreamNumbers.Services;

namespace DreamNumbers.ScoringStrategies;

public class HazardRateStrategy : IScoringStrategy
{
    public string Name => "Hazard Rate Strategy";

    public Dictionary<int, double> CalculateMainNumberScores(
        List<Draw> draws,
        List<NumberStatistics> stats,
        int interval)
    {
        return CalculateScores(
            draws,
            stats.Select(s => s.Number),
            (draw, number) => draw.Numbers.Contains(number)
        );
    }

    public Dictionary<int, double> CalculateDreamNumberScores(
        List<Draw> draws,
        List<DreamNumberStatistics> stats,
        int interval)
    {
        return CalculateScores(
            draws,
            stats.Select(s => s.Number),
            (draw, number) => draw.DreamNumber == number
        );
    }

    private Dictionary<int, double> CalculateScores(
        List<Draw> draws,
        IEnumerable<int> universe,
        Func<Draw, int, bool> appears)
    {
        var scores = new Dictionary<int, double>();
        var ordered = draws.OrderBy(d => d.Date).ToList();

        foreach (var number in universe)
        {
            var hazardTable = new Dictionary<int, (int total, int hits)>();
            int absence = 0;

            foreach (var draw in ordered)
            {
                if (!hazardTable.ContainsKey(absence))
                    hazardTable[absence] = (0, 0);

                var entry = hazardTable[absence];
                entry.total++;

                if (appears(draw, number))
                {
                    entry.hits++;
                    absence = 0;
                }
                else
                {
                    absence++;
                }

                hazardTable[absence] = entry;
            }

            int currentAbsence = ordered
                .Reverse<Draw>()
                .TakeWhile(d => !appears(d, number))
                .Count();

            if (hazardTable.TryGetValue(currentAbsence, out var data))
            {
                scores[number] = data.total > 0
                    ? (double)data.hits / data.total
                    : 0;
            }
            else
            {
                scores[number] = 0;
            }
        }

        return scores;
    }
}
