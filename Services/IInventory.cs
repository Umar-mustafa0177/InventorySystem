using System;
using System.Collections.Generic;
using InventorySystem.Models;

namespace InventorySystem.Services
{
    // Delegate for event handling
    public delegate void StockAlertHandler(string message);

    public interface IInventory
    {
        // CRUD Operations
        void AddProduct(Product p);
        Product GetProductById(int id);
        List<Product> GetAllProducts();
        void UpdateProduct(int id);
        void DeleteProduct(int id);

        // Business Operations
        void SellProduct(int id, int quantity);
        void AddStock(int id, int quantity);

        // Search & Filter
        List<Product> SearchProducts(string keyword);
        List<Product> FilterByCategory(string category);
        List<Product> GetLowStockProducts();
        List<Product> GetExpiredProducts();

        // Analytics
        double GetTotalInventoryValue();
        double GetCategoryValue(string category);
        Dictionary<string, int> GetStockDistribution();

        // Events
        event StockAlertHandler OnLowStock;

        // File Operations removed - persistence handled externally if needed
    }
}