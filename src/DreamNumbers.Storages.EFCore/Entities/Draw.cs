using System;
using System.Collections.Generic;
using System.Text;

namespace DreamNumbers.Storages.EFCore.Entities
{
    internal class Draw
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<int> Numbers { get; set; } = new();
    }
}
