using System;
using System.Collections.Generic;

namespace InventorySystem.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public List<int> SuppliedProducts { get; set; } = new List<int>();

        public void Display()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[Supplier] {Name} | Contact: {ContactPerson} | {Phone}");
            Console.ResetColor();
        }
    }
}