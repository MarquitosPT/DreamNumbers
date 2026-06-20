namespace DreamNumbers.Models
{
    public class EuroMillionsSimulationResult
    {
        public List<EuroMillionsSimulatedCombination> Combinations { get; set; } = [];
        public Dictionary<int, double> NumberScores { get; set; } = [];
        public Dictionary<int, double> StarScores { get; set; } = [];
    }

    public class EuroMillionsSimulatedCombination
    {
        public List<int> Numbers { get; set; } = [];
        public List<int> Stars { get; set; } = [];
    }
}
