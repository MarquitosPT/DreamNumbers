namespace DreamNumbers.Services.EUDreams.Models
{
    public class EuroDreamsResult
    {
        public DateTime Date { get; set; }
        public string DrawNumber { get; set; } = string.Empty;
        public List<int> Numbers { get; set; } = new();
        public int DreamNumber { get; set; }
    }
}
