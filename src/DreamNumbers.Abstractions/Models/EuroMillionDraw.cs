namespace DreamNumbers.Models
{
    public class EuroMillionDraw
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string DrawNumber { get; set; } = string.Empty;

        public List<int> Numbers { get; set; } = [];

        public List<int> Stars { get; set; } = [];
    }
}
