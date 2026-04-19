using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using InventorySystem.Models;
using InventorySystem.Exceptions;

namespace InventorySystem.Services
{
    public class InventoryManager : IInventory
    {
        private List<Product> _products = new List<Product>();
        private List<Supplier> _suppliers = new List<Supplier>();
        private int _nextProductId = 1;

        // Event declaration
        public event StockAlertHandler OnLowStock;

        public InventoryManager()
        {
            // No persistent file loading - in-memory only
        }

        // Generate new ID
        private int GetNewId() => _nextProductId++;

        // ADD PRODUCT
        public void AddProduct(Product p)
        {
            try
            {
                // Validate
                if (string.IsNullOrWhiteSpace(p.Name))
                    throw new Exception("Product name cannot be empty");
                if (p.Price <= 0)
                    throw new Exception("Price must be greater than 0");

                p.Id = GetNewId();
                p.CreatedDate = DateTime.Now;
                _products.Add(p);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ Product '{p.Name}' added successfully! (ID: {p.Id})");
                Console.ResetColor();

                // Check stock alert
                if (p.GetStockStatus() == "LOW STOCK ⚠️")
                    OnLowStock?.Invoke($"Low stock alert for {p.Name}! Only {p.Quantity} left.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Error: {ex.Message}");
                Console.ResetColor();
            }
        }

        // GET PRODUCT BY ID
        public Product GetProductById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                throw new ProductNotFoundException($"Product with ID {id} not found");
            return product;
        }

        // GET ALL PRODUCTS
        public List<Product> GetAllProducts() => _products;

        // UPDATE PRODUCT
        public void UpdateProduct(int id)
        {
            try
            {
                var product = GetProductById(id);

                Console.WriteLine($"Updating: {product.Name}");
                Console.Write("New Name (press enter to skip): ");
                string name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name)) product.Name = name;

                Console.Write("New Price (press enter to skip): ");
                string priceInput = Console.ReadLine();
                if (double.TryParse(priceInput, out double price) && price > 0)
                    product.Price = price;

                Console.Write("New Quantity (press enter to skip): ");
                string qtyInput = Console.ReadLine();
                if (int.TryParse(qtyInput, out int qty) && qty >= 0)
                    product.Quantity = qty;

                // Update specific fields based on type
                if (product is Electronics e)
                {
                    Console.Write("New Brand: ");
                    string brand = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(brand)) e.Brand = brand;
                }
                else if (product is Grocery g)
                {
                    Console.Write("New Expiry Date (yyyy-mm-dd): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime expiry))
                        g.ExpiryDate = expiry;
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✅ Product updated successfully!");
                Console.ResetColor();
            }
            catch (ProductNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ {ex.Message}");
                Console.ResetColor();
            }
        }

        // DELETE PRODUCT
        public void DeleteProduct(int id)
        {
            try
            {
                var product = GetProductById(id);
                _products.Remove(product);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ Product '{product.Name}' deleted!");
                Console.ResetColor();
            }
            catch (ProductNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ {ex.Message}");
                Console.ResetColor();
            }
        }

        // SELL PRODUCT
        public void SellProduct(int id, int quantity)
        {
            try
            {
                if (quantity <= 0)
                    throw new InvalidQuantityException("Sell quantity must be positive");

                var product = GetProductById(id);

                // Check for expired grocery
                if (product is Grocery grocery && grocery.IsExpired())
                    throw new ExpiredProductException($"Cannot sell expired product: {grocery.Name}");

                if (product.Quantity < quantity)
                    throw new InsufficientStockException($"Only {product.Quantity} units available");

                product.Quantity -= quantity;
                double totalAmount = product.Price * quantity;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ Sold {quantity} x {product.Name}");
                Console.WriteLine($"💰 Total: ${totalAmount:F2}");
                Console.ResetColor();

                // Check stock alert after sale
                if (product.Quantity < 5)
                    OnLowStock?.Invoke($"⚠️ {product.Name} is running low! Only {product.Quantity} left.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Error: {ex.Message}");
                Console.ResetColor();
            }
        }

        // ADD STOCK
        public void AddStock(int id, int quantity)
        {
            try
            {
                if (quantity <= 0)
                    throw new InvalidQuantityException("Add quantity must be positive");

                var product = GetProductById(id);
                product.Quantity += quantity;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ Added {quantity} units to {product.Name}. New stock: {product.Quantity}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Error: {ex.Message}");
                Console.ResetColor();
            }
        }

        // SEARCH PRODUCTS
        public List<Product> SearchProducts(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return _products;

            return _products.Where(p =>
                p.Name.ToLower().Contains(keyword.ToLower()) ||
                p.Category.ToLower().Contains(keyword.ToLower())
            ).ToList();
        }

        // FILTER BY CATEGORY
        public List<Product> FilterByCategory(string category)
        {
            return _products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // GET LOW STOCK PRODUCTS
        public List<Product> GetLowStockProducts()
        {
            return _products.Where(p => p.Quantity < 5 && p.Quantity > 0).ToList();
        }

        // GET EXPIRED PRODUCTS
        public List<Product> GetExpiredProducts()
        {
            return _products.Where(p => p is Grocery g && g.IsExpired()).ToList();
        }

        // TOTAL INVENTORY VALUE
        public double GetTotalInventoryValue()
        {
            return _products.Sum(p => p.Price * p.Quantity);
        }

        // CATEGORY VALUE
        public double GetCategoryValue(string category)
        {
            return _products.Where(p => p.Category == category).Sum(p => p.Price * p.Quantity);
        }

        // STOCK DISTRIBUTION
        public Dictionary<string, int> GetStockDistribution()
        {
            return _products.GroupBy(p => p.Category)
                           .ToDictionary(g => g.Key, g => g.Sum(p => p.Quantity));
        }

        // DISPLAY ALL PRODUCTS (Formatted)
        public void DisplayAllProducts()
        {
            if (_products.Count == 0)
            {
                Console.WriteLine("📭 No products in inventory.");
                return;
            }

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine($"📦 INVENTORY SUMMARY (Total: {_products.Count} products)");
            Console.WriteLine(new string('=', 60));

            foreach (var product in _products)
            {
                product.Display();
                Console.WriteLine();
            }

            Console.WriteLine(new string('=', 60));
            Console.WriteLine($"💰 Total Inventory Value: ${GetTotalInventoryValue():F2}");
            Console.WriteLine(new string('=', 60));
        }

        // File persistence removed - Inventory is in-memory only
    }
}