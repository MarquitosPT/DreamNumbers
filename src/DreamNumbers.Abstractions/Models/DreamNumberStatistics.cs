using System;
using System.Collections.Generic;
using System.Text;

namespace DreamNumbers.Models
{
    public class DreamNumberStatistics
    {
        public int Number { get; set; } // 1 a 5
        public int Frequency20 { get; set; }
        public int Frequency40 { get; set; }
        public int Frequency60 { get; set; }
        public int CurrentAbsence { get; set; }
        public double EstimatedProbability { get; set; }
    }
}
