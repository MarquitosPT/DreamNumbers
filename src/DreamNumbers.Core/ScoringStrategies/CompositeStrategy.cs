using DreamNumbers.Models;
using DreamNumbers.Services;

namespace DreamNumbers.ScoringStrategies;

public class CompositeStrategy : IScoringStrategy
{
    public string Name => "Composite Strategy";

    private readonly List<(IScoringStrategy strategy, double weight)> _strategies;

    public CompositeStrategy()
    {
        _strategies = [];
    }

    public CompositeStrategy(params (IScoringStrategy strategy, double weight)[] strategies)
    {
        _strategies = strategies.ToList();
    }

    public Dictionary<int, double> CalculateMainNumberScores(
        List<Draw> draws,
        List<NumberStatistics> stats,
        int interval)
    {
        return Combine(s => s.CalculateMainNumberScores(draws, stats, interval));
    }

    public Dictionary<int, double> CalculateDreamNumberScores(
        List<Draw> draws,
        List<DreamNumberStatistics> stats,
        int interval)
    {
        return Combine(s => s.CalculateDreamNumberScores(draws, stats, interval));
    }

    private Dictionary<int, double> Combine(Func<IScoringStrategy, Dictionary<int, double>> selector)
    {
        var final = new Dictionary<int, double>();

        foreach (var (strategy, weight) in _strategies)
        {
            var scores = selector(strategy);

            foreach (var kv in scores)
            {
                if (!final.ContainsKey(kv.Key))
                    final[kv.Key] = 0;

                final[kv.Key] += kv.Value * weight;
            }
        }

        return final;
    }
}
