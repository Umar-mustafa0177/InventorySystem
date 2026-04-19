using System;
using System.Xml.Serialization;
using InventorySystem.Exceptions;

namespace InventorySystem.Models
{
    [XmlInclude(typeof(Electronics))]
    [XmlInclude(typeof(Grocery))]
    public abstract class Product
    {
        // Properties with validation
        private int _quantity;

        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value < 0)
                    throw new InvalidQuantityException("Quantity cannot be negative");
                _quantity = value;
            }
        }

        public DateTime CreatedDate { get; set; }
        public string Category { get; protected set; }

        // Virtual method - can be overridden
        public virtual double CalculateDiscount()
        {
            return Price; // No discount by default
        }

        // Abstract method - must be implemented
        public abstract string GetProductType();

        // Stock status check
        public virtual string GetStockStatus()
        {
            if (Quantity <= 0) return "OUT OF STOCK";
            if (Quantity < 5) return "LOW STOCK ⚠️";
            if (Quantity < 20) return "MEDIUM";
            return "HIGH";
        }

        // Display method with formatting
        public virtual void Display()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"┌─────────────────────────────────────────┐");
            Console.WriteLine($"│ ID: {Id,-5} | {GetProductType(),-12}         │");
            Console.WriteLine($"│ Name: {Name,-30} │");
            Console.WriteLine($"│ Price: ${Price,-8} | Qty: {Quantity,-5}        │");
            Console.WriteLine($"│ Stock Status: {GetStockStatus(),-20}│");
            Console.WriteLine($"│ Added: {CreatedDate.ToShortDateString(),-25}│");
            Console.WriteLine($"└─────────────────────────────────────────┘");
            Console.ResetColor();
        }
    }
}