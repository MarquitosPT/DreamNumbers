namespace DreamNumbers.Services.JSC.Models
{
    public class EuroMillionsResult
    {
        public int ContestNumber { get; set; }
        public DateTime Date { get; set; }
        public string DrawNumber { get; set; } = string.Empty;
        public List<int> Numbers { get; set; } = [];
        public List<int> Stars { get; set; } = [];
    }
}
