using System;
using System.Linq;
using InventorySystem.Models;

namespace InventorySystem.Services
{
    public static class ReportGenerator
    {
        public static void GenerateSalesReport(InventoryManager manager)
        {
            var products = manager.GetAllProducts();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n" + new string('=', 70));
            Console.WriteLine("📊 INVENTORY ANALYTICS REPORT");
            Console.WriteLine(new string('=', 70));
            Console.ResetColor();

            // Category breakdown
            var electronics = products.Where(p => p.Category == "Electronics").Sum(p => p.Price * p.Quantity);
            var groceries = products.Where(p => p.Category == "Grocery").Sum(p => p.Price * p.Quantity);

            Console.WriteLine($"\n📈 Category Value Breakdown:");
            Console.WriteLine($"   Electronics: ${electronics:F2}");
            Console.WriteLine($"   Groceries:   ${groceries:F2}");
            Console.WriteLine($"   TOTAL:       ${manager.GetTotalInventoryValue():F2}");

            // Stock alerts
            var lowStock = manager.GetLowStockProducts();
            if (lowStock.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n⚠️ LOW STOCK ALERTS ({lowStock.Count} products):");
                foreach (var p in lowStock)
                    Console.WriteLine($"   - {p.Name}: {p.Quantity} units left");
                Console.ResetColor();
            }

            // Expired products
            var expired = manager.GetExpiredProducts();
            if (expired.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n❌ EXPIRED PRODUCTS ({expired.Count} products):");
                foreach (var p in expired)
                    Console.WriteLine($"   - {p.Name} (Expired!)");
                Console.ResetColor();
            }

            // Stock distribution chart
            Console.WriteLine($"\n📊 Stock Distribution:");
            var distribution = manager.GetStockDistribution();
            foreach (var kvp in distribution)
            {
                int barLength = Math.Min(kvp.Value / 5, 40);
                string bar = new string('█', barLength);
                Console.WriteLine($"   {kvp.Key,-12}: {bar} {kvp.Value}");
            }
        }
    }
}