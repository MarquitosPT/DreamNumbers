namespace DreamNumbers.Models
{
    public class Draw
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public int Year { get; set; }

        public DateTime Date { get; set; }
        public List<int> Numbers { get; set; } = new();

        public int DreamNumber { get; set; }
    }

}
