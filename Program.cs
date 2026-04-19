using System;
using InventorySystem.Models;
using InventorySystem.Services;

class Program
{
    static InventoryManager inventory = new InventoryManager();

    static void Main()
    {
        // Subscribe to stock alert event
        inventory.OnLowStock += (message) =>
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n🔔 EVENT: {message}");
            Console.ResetColor();
        };

        Console.Title = "Advanced Inventory Management System";

        while (true)
        {
            ShowMainMenu();

            int choice = GetValidInput("Choose option: ", 1, 12);

            switch (choice)
            {
                case 1: AddElectronics(); break;
                case 2: AddGrocery(); break;
                case 3: inventory.DisplayAllProducts(); break;
                case 4: UpdateProduct(); break;
                case 5: DeleteProduct(); break;
                case 6: SearchProducts(); break;
                case 7: SellProduct(); break;
                case 8: AddStock(); break;
                case 9: ShowReports(); break;
                case 10: ShowAnalytics(); break;
                case 11: ShowHelp(); break;
                case 12: ExitApp(); break;
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    static void ShowMainMenu()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
╔══════════════════════════════════════════════════════════╗
║     ADVANCED INVENTORY MANAGEMENT SYSTEM v2.0            ║
║              (Final Year Project)                        ║
╚══════════════════════════════════════════════════════════╝");
        Console.ResetColor();

        Console.WriteLine(@"
📦 PRODUCT MANAGEMENT:
   1. Add Electronics
   2. Add Grocery
   3. View All Products
   4. Update Product
   5. Delete Product

🔍 SEARCH & FILTER:
   6. Search Products

💰 SALES & STOCK:
   7. Sell Product
   8. Add Stock

📊 REPORTS:
   9. Generate Reports
   10. Analytics Dashboard

ℹ️  OTHER:
   11. Help / OOP Concepts
   12. Exit
");
        Console.Write("👉 ");
    }

    static void AddElectronics()
    {
        Console.Clear();
        Console.WriteLine("--- ADD NEW ELECTRONICS ---\n");

        Electronics e = new Electronics();

        Console.Write("Name: ");
        e.Name = Console.ReadLine();

        Console.Write("Price: $");
        e.Price = GetValidDouble();

        Console.Write("Quantity: ");
        e.Quantity = GetValidInt();

        Console.Write("Brand: ");
        e.Brand = Console.ReadLine();

        Console.Write("Warranty (months): ");
        e.WarrantyMonths = GetValidInt();

        Console.Write("Is Refurbished? (y/n): ");
        e.IsRefurbished = Console.ReadLine().ToLower() == "y";

        inventory.AddProduct(e);
    }

    static void AddGrocery()
    {
        Console.Clear();
        Console.WriteLine("--- ADD NEW GROCERY ---\n");

        Grocery g = new Grocery();

        Console.Write("Name: ");
        g.Name = Console.ReadLine();

        Console.Write("Price: $");
        g.Price = GetValidDouble();

        Console.Write("Quantity: ");
        g.Quantity = GetValidInt();

        Console.Write("Weight (kg): ");
        g.WeightInKg = GetValidDouble();

        Console.Write("Expiry Date (yyyy-mm-dd): ");
        g.ExpiryDate = DateTime.Parse(Console.ReadLine());

        Console.Write("Is Organic? (y/n): ");
        g.IsOrganic = Console.ReadLine().ToLower() == "y";

        inventory.AddProduct(g);
    }

    static void UpdateProduct()
    {
        Console.Clear();
        Console.Write("Enter Product ID to update: ");
        int id = GetValidInt();
        inventory.UpdateProduct(id);
    }

    static void DeleteProduct()
    {
        Console.Clear();
        Console.Write("Enter Product ID to delete: ");
        int id = GetValidInt();
        inventory.DeleteProduct(id);
    }

    static void SearchProducts()
    {
        Console.Clear();
        Console.Write("Enter search keyword: ");
        string keyword = Console.ReadLine();

        var results = inventory.SearchProducts(keyword);

        Console.WriteLine($"\n🔍 Found {results.Count} product(s):\n");
        foreach (var p in results)
            p.Display();
    }

    static void SellProduct()
    {
        Console.Clear();
        Console.Write("Product ID: ");
        int id = GetValidInt();

        Console.Write("Quantity to sell: ");
        int qty = GetValidInt();

        inventory.SellProduct(id, qty);
    }

    static void AddStock()
    {
        Console.Clear();
        Console.Write("Product ID: ");
        int id = GetValidInt();

        Console.Write("Quantity to add: ");
        int qty = GetValidInt();

        inventory.AddStock(id, qty);
    }

    static void ShowReports()
    {
        Console.Clear();
        ReportGenerator.GenerateSalesReport(inventory);
    }

    static void ShowAnalytics()
    {
        Console.Clear();
        Console.WriteLine("📊 ADVANCED ANALYTICS\n");

        var products = inventory.GetAllProducts();

        Console.WriteLine($"📌 Total Products: {products.Count}");
        Console.WriteLine($"💰 Total Value: ${inventory.GetTotalInventoryValue():F2}");
        Console.WriteLine($"📦 Electronics Value: ${inventory.GetCategoryValue("Electronics"):F2}");
        Console.WriteLine($"🥫 Grocery Value: ${inventory.GetCategoryValue("Grocery"):F2}");

        var lowStock = inventory.GetLowStockProducts();
        Console.WriteLine($"⚠️  Low Stock Items: {lowStock.Count}");

        var expired = inventory.GetExpiredProducts();
        Console.WriteLine($"❌ Expired Items: {expired.Count}");

        if (expired.Count > 0)
        {
            Console.WriteLine("\n🚨 URGENT - Expired products to dispose:");
            foreach (var p in expired)
                Console.WriteLine($"   - {p.Name}");
        }
    }

    static void ShowHelp()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(@"
╔═══════════════════════════════════════════════════════════════╗
║                    OOP CONCEPTS DEMONSTRATED                  ║
╠═══════════════════════════════════════════════════════════════╣
║                                                               ║
║  1. ENCAPSULATION                                            ║
║     → Private fields with public properties                  ║
║     → Validation in property setters                         ║
║                                                               ║
║  2. INHERITANCE                                              ║
║     → Electronics and Grocery inherit from Product           ║
║     → Code reusability                                       ║
║                                                               ║
║  3. POLYMORPHISM                                             ║
║     → Virtual Display() overridden in child classes          ║
║     → Abstract GetProductType() method                       ║
║     → Runtime polymorphism                                   ║
║                                                               ║
║  4. ABSTRACTION                                              ║
║     → Abstract Product class                                 ║
║     → IInventory interface                                   ║
║                                                               ║
║  5. INTERFACE                                                ║
║     → IInventory defines contract                            ║
║     → InventoryManager implements it                         ║
║                                                               ║
║  6. DELEGATES & EVENTS                                       ║
║     → StockAlertHandler delegate                             ║
║     → OnLowStock event for notifications                     ║
║                                                               ║
║  7. EXCEPTION HANDLING                                       ║
║     → Custom exceptions (ProductNotFoundException, etc.)     ║
║     → Try-catch blocks                                       ║
║                                                               ║
║  8. GENERICS & LINQ                                          ║
║     → List<T> collections                                    ║
║     → LINQ queries (Where, Select, Sum, GroupBy)             ║
║                                                               ║
╚═══════════════════════════════════════════════════════════════╝");
        Console.ResetColor();
    }

    static void ExitApp()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n👋 Goodbye!");
        Console.ResetColor();
        Environment.Exit(0);
    }

    // Helper methods for input validation
    static int GetValidInt()
    {
        int result;
        while (!int.TryParse(Console.ReadLine(), out result))
            Console.Write("Invalid! Enter number: ");
        return result;
    }

    static int GetValidInput(string prompt, int min, int max)
    {
        int result;
        do
        {
            Console.Write(prompt);
            result = GetValidInt();
        } while (result < min || result > max);
        return result;
    }

    static double GetValidDouble()
    {
        double result;
        while (!double.TryParse(Console.ReadLine(), out result) || result <= 0)
            Console.Write("Invalid! Enter positive number: ");
        return result;
    }
}