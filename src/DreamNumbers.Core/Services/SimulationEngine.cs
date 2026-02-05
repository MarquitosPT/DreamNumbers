using System.Security.Cryptography;
using DreamNumbers.Models;

namespace DreamNumbers.Services;

public class SimulationEngine : ISimulationEngine
{
    public SimulationResult Generate(
        int interval,
        int combinations,
        IScoringStrategy strategy,
        List<Draw> draws,
        List<NumberStatistics> mainStats,
        List<DreamNumberStatistics> dreamStats)
    {
        var mainScores = strategy.CalculateMainNumberScores(draws, mainStats, interval);
        var dreamScores = strategy.CalculateDreamNumberScores(draws, dreamStats, interval);

        var normalizedMain = NormalizeScores(mainScores);
        var normalizedDream = NormalizeScores(dreamScores);

        var result = new SimulationResult();

        for (int i = 0; i < combinations; i++)
        {
            var numbers = WeightedPick(normalizedMain, 6);
            var dream = WeightedPick(normalizedDream, 1).First();

            result.Combinations.Add(new SimulatedCombination
            {
                Numbers = numbers.OrderBy(n => n).ToList(),
                DreamNumber = dream
            });
        }

        return result;
    }

    private Dictionary<int, double> NormalizeScores(Dictionary<int, double> scores)
    {
        // Se todos forem zero, distribui uniformemente
        if (scores.Values.All(v => v <= 0))
        {
            double uniform = 1.0 / scores.Count;
            return scores.Keys.ToDictionary(k => k, _ => uniform);
        }

        // Shift para evitar negativos
        double min = scores.Values.Min();
        if (min < 0)
        {
            scores = scores.ToDictionary(kv => kv.Key, kv => kv.Value - min);
        }

        double sum = scores.Values.Sum();
        if (sum == 0)
        {
            double uniform = 1.0 / scores.Count;
            return scores.Keys.ToDictionary(k => k, _ => uniform);
        }

        return scores.ToDictionary(kv => kv.Key, kv => kv.Value / sum);
    }

    private List<int> WeightedPick(Dictionary<int, double> weights, int count)
    {
        var picked = new HashSet<int>();
        var list = weights.ToList();

        while (picked.Count < count && picked.Count < list.Count)
        {
            int chosen = WeightedSinglePick(list, picked);
            picked.Add(chosen);
        }

        return picked.ToList();
    }

    private int WeightedSinglePick(List<KeyValuePair<int, double>> weights, HashSet<int> exclude)
    {
        var filtered = weights.Where(kv => !exclude.Contains(kv.Key)).ToList();
        double total = filtered.Sum(kv => kv.Value);
        if (total == 0)
        {
            return filtered[RandomNumberGenerator.GetInt32(filtered.Count)].Key;
        }

        double r = NextDouble() * total;
        double acc = 0;

        foreach (var kv in filtered)
        {
            acc += kv.Value;
            if (r <= acc)
                return kv.Key;
        }

        return filtered.Last().Key;
    }

    private double NextDouble()
    {
        Span<byte> bytes = stackalloc byte[8];
        RandomNumberGenerator.Fill(bytes);
        ulong ul = BitConverter.ToUInt64(bytes);
        return (ul / (double)ulong.MaxValue);
    }
}
