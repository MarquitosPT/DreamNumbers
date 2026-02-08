namespace DreamNumbers.Models
{
    public class SimulatedCombination
    {
        public List<int> Numbers { get; set; } = new();
        public int DreamNumber { get; set; }
    }

    public class SimulationResult
    {
        public List<SimulatedCombination> Combinations { get; set; } = new();

        public Dictionary<int, double> MainScores { get; set; } = new();
        public Dictionary<int, double> DreamScores { get; set; } = new();
    }
}
