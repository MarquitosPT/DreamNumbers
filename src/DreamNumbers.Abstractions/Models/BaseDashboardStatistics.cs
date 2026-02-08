namespace DreamNumbers.Models
{
    public class BaseDashboardStatistics
    {
        public int Number { get; set; }
        public int CurrentAbsence { get; set; } = -1;
        public int Frequency20 { get; set; }
        public int Frequency40 { get; set; }
        public int Frequency60 { get; set; }
        public double EstimatedProbability { get; set; }
    }
}
