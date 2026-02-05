using System;
using System.Collections.Generic;
using System.Text;
using DreamNumbers.Models;

namespace DreamNumbers.Services
{
    public interface IScoringStrategy
    {
        string Name { get; }

        /// <summary>
        /// Calcula scores para números principais (1–40).
        /// </summary>
        Dictionary<int, double> CalculateMainNumberScores(
            List<Draw> draws,
            List<NumberStatistics> stats,
            int interval);

        /// <summary>
        /// Calcula scores para DreamNumber (1–5).
        /// </summary>
        Dictionary<int, double> CalculateDreamNumberScores(
            List<Draw> draws,
            List<DreamNumberStatistics> stats,
            int interval);
    }
}
