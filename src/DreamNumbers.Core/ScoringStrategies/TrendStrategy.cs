using DreamNumbers.Models;
using DreamNumbers.Services;

namespace DreamNumbers.ScoringStrategies;

public class TrendStrategy : IScoringStrategy
{
    public string Name => "Trend Strategy";

    public Dictionary<int, double> CalculateMainNumberScores(
        List<Draw> draws,
        List<NumberStatistics> stats,
        int interval)
    {
        var scores = new Dictionary<int, double>();

        foreach (var s in stats)
        {
            scores[s.Number] = CalculateTrend(
                s.Frequency20,
                s.Frequency40,
                s.Frequency60
            );
        }

        return scores;
    }

    public Dictionary<int, double> CalculateDreamNumberScores(
        List<Draw> draws,
        List<DreamNumberStatistics> stats,
        int interval)
    {
        var scores = new Dictionary<int, double>();

        foreach (var s in stats)
        {
            scores[s.Number] = CalculateTrend(
                s.Frequency20,
                s.Frequency40,
                s.Frequency60
            );
        }

        return scores;
    }

    private double CalculateTrend(int f20, int f40, int f60)
    {
        // Pontos da regressão linear
        double[] x = { 20, 40, 60 };
        double[] y = { f20, f40, f60 };

        double avgX = x.Average();
        double avgY = y.Average();

        double num = 0;
        double den = 0;

        for (int i = 0; i < x.Length; i++)
        {
            num += (x[i] - avgX) * (y[i] - avgY);
            den += Math.Pow(x[i] - avgX, 2);
        }

        // Slope da regressão linear
        return den == 0 ? 0 : num / den;
    }
}
