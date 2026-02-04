namespace DreamNumbers.Storages.EFCore.Entities
{
    internal class Draw
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string DrawNumber { get; set; } = string.Empty;

        public List<int> Numbers { get; set; } = [];

        public int DreamNumber { get; set; }
    }
}
