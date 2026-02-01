using DreamNumbers.Models;

namespace DreamNumbers.Services
{
    internal class StatisticsService : IStatisticsService
    {
        public List<NumberStatistics> CalculateStatistics(List<Draw> draws)
        {
            var stats = new List<NumberStatistics>();

            for (int number = 1; number <= 40; number++)
            {
                var s = new NumberStatistics
                {
                    Number = number,
                    Frequency20 = CalculateFrequency(draws, number, 20),
                    Frequency40 = CalculateFrequency(draws, number, 40),
                    Frequency60 = CalculateFrequency(draws, number, 60),
                    CurrentAbsence = CalculateCurrentAbsence(draws, number)
                };

                s.EstimatedProbability = CalculateProbability(s);

                stats.Add(s);
            }

            return stats;
        }

        private int CalculateFrequency(List<Draw> draws, int number, int interval)
        {
            return draws
                .TakeLast(interval)
                .Count(d => d.Numbers.Contains(number));
        }

        private int CalculateCurrentAbsence(List<Draw> draws, int number)
        {
            int count = 0;

            foreach (var draw in draws.OrderByDescending(d => d.Date))
            {
                if (draw.Numbers.Contains(number))
                    break;

                count++;
            }

            return count;
        }

        private double CalculateProbability(NumberStatistics s)
        {
            // Weighted probability model:
            // Higher absence → higher probability
            // Frequencies also influence the weight

            double absenceWeight = s.CurrentAbsence * 1.0;
            double frequencyWeight =
                  (s.Frequency20 * 0.5)
                + (s.Frequency40 * 0.3)
                + (s.Frequency60 * 0.2);

            double score = absenceWeight + frequencyWeight;

            // Normalize to a probability between 0 and 1
            double maxPossible = (60 * 1.0) + (20 * 0.5 + 40 * 0.3 + 60 * 0.2);

            return score / maxPossible;
        }
    }
}
