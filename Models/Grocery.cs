using System;

namespace InventorySystem.Models
{
    public class Grocery : Product
    {
        public DateTime ExpiryDate { get; set; }
        public bool IsOrganic { get; set; }
        public double WeightInKg { get; set; }

        public Grocery()
        {
            Category = "Grocery";
            CreatedDate = DateTime.Now;
        }

        public override double CalculateDiscount()
        {
            // Check if product is near expiry (within 7 days)
            if ((ExpiryDate - DateTime.Now).Days <= 7)
                return Price * 0.50; // 50% discount
            else if (IsOrganic)
                return Price * 0.95; // 5% discount
            else
                return Price * 0.98; // 2% discount
        }

        public bool IsExpired()
        {
            return DateTime.Now > ExpiryDate;
        }

        public override string GetProductType()
        {
            string type = IsOrganic ? "Organic " : "";
            type += "Grocery";
            if (IsExpired()) type += " (EXPIRED!)";
            return type;
        }

        public override string GetStockStatus()
        {
            if (IsExpired()) return "EXPIRED - DISPOSE!";
            return base.GetStockStatus();
        }

        public override void Display()
        {
            base.Display();
            Console.ForegroundColor = IsExpired() ? ConsoleColor.Red : ConsoleColor.Green;
            Console.WriteLine($"│ Expiry: {ExpiryDate.ToShortDateString(),-10} | Weight: {WeightInKg}kg      │");
            Console.WriteLine($"│ Discounted Price: ${CalculateDiscount():F2,-25}│");
            if (IsExpired())
                Console.WriteLine($"│ ⚠️  EXPIRED - Please dispose!              │");
            Console.ResetColor();
        }
    }
}