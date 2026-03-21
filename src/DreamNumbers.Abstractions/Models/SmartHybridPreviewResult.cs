using System;
using System.Collections.Generic;
using System.Text;

namespace DreamNumbers.Models
{
    public sealed class SmartHybridPreviewResult
    {
        public List<int> Numbers { get; set; } = new();
        public int DreamNumber { get; set; }

        public Dictionary<int, double> MainScores { get; set; } = new();
        public Dictionary<int, double> DreamScores { get; set; } = new();
        public Dictionary<int, double> AdjustedMainScores { get; set; } = new();

        public double GlobalPenalty { get; set; }
        public double Similarity { get; set; }

        
    }
}
