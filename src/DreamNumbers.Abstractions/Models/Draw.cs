namespace DreamNumbers.Models
{
    public class Draw
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<int> Numbers { get; set; } = new();
    }

}
