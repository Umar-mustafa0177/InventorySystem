using System;

namespace InventorySystem.Models
{
    public class Electronics : Product
    {
        public string Brand { get; set; }
        public int WarrantyMonths { get; set; }
        public bool IsRefurbished { get; set; }

        public Electronics()
        {
            Category = "Electronics";
            CreatedDate = DateTime.Now;
        }

        public override double CalculateDiscount()
        {
            // Electronics: 10% discount if not refurbished
            if (IsRefurbished)
                return Price * 0.95; // 5% discount only
            else
                return Price * 0.90; // 10% discount
        }

        public override string GetProductType()
        {
            return IsRefurbished ? "Refurbished Electronics" : "New Electronics";
        }

        public override void Display()
        {
            base.Display();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"│ Brand: {Brand,-15} | Warranty: {WarrantyMonths} months │");
            Console.WriteLine($"│ Discounted Price: ${CalculateDiscount():F2,-25}│");
            Console.ResetColor();
        }
    }
}