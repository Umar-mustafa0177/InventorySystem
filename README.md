# 🚀 Advanced Inventory Management System

A production-ready **Inventory Management System** built with **C#** and **.NET 8.0** demonstrating real-world implementation of Object-Oriented Programming concepts including Inheritance, Polymorphism, Abstraction, Encapsulation, Interfaces, Delegates, Events, and LINQ.

---

## 📋 Table of Contents

- [Features](#features)
- [Technology Stack](#technology-stack)
- [OOP Concepts Demonstrated](#oop-concepts-demonstrated)
- [Project Structure](#project-structure)
- [Installation & Setup](#installation--setup)
- [How to Use](#how-to-use)
- [Code Examples](#code-examples)
- [Screenshots](#screenshots)
- [Author](#author)
- [License](#license)

---

## ✨ Features

### Core Functionality
| # | Feature | Description |
|---|---------|-------------|
| 1 | Add Electronics | Add electronic products with brand, warranty period, and refurbished status |
| 2 | Add Grocery | Add grocery items with expiry date, organic certification, and weight |
| 3 | View Products | Display all products with formatted output and stock status indicators |
| 4 | Update Product | Modify existing product details including price and quantity |
| 5 | Delete Product | Remove products from inventory by ID |
| 6 | Search Product | Search products by name or category using LINQ queries |
| 7 | Sell Product | Process sales with automatic stock reduction and validation |
| 8 | Total Value | Calculate total inventory value across all categories |

### Advanced Features
- ⚠️ **Low Stock Alerts**: Automatic notifications when stock falls below 5 units
- 🔔 **Event System**: Real-time alerts using delegates and events
- 🔍 **LINQ Queries**: Advanced searching, filtering, and aggregation
- 📊 **Stock Status**: Real-time stock level indicators (HIGH/MEDIUM/LOW/OUT OF STOCK)
- 🏷️ **Category Management**: Separate handling for Electronics and Grocery categories
- ⏰ **Expiry Tracking**: Automatic detection of expired grocery items

---

## 🛠️ Technology Stack

| Component | Technology |
|-----------|------------|
| Language | C# 12.0 |
| Framework | .NET 8.0 |
| IDE | Visual Studio 2022 / VS Code |
| Version Control | Git & GitHub |

---

## 🎓 OOP Concepts Demonstrated

| Concept | Implementation | Location |
|---------|---------------|----------|
| **Encapsulation** | Private fields with public properties, validation in setters | `Product.cs` |
| **Inheritance** | `Electronics` and `Grocery` inherit from `Product` | `Electronics.cs`, `Grocery.cs` |
| **Polymorphism** | Virtual `Display()` and `CalculateDiscount()` methods overridden | All model classes |
| **Abstraction** | Abstract `Product` class with abstract `GetProductType()` method | `Product.cs` |
| **Interface** | `IInventory` interface defining contract for all operations | `IInventory.cs` |
| **Delegates** | `StockAlertHandler` delegate for event handling | `IInventory.cs` |
| **Events** | `OnLowStock` event triggered when inventory runs low | `InventoryManager.cs` |
| **Generics** | `List<T>` collection for storing products | `InventoryManager.cs` |
| **LINQ** | `Where()`, `Sum()`, `FirstOrDefault()` queries | `InventoryManager.cs` |
| **Exception Handling** | Custom exceptions with try-catch blocks | `InventoryExceptions.cs` |

---

## 📁 Project Structure

```
Advanced-Inventory-System/
│
├── Program.cs                          # Main entry point, menu UI, user input
│
├── Models/
│   ├── Product.cs                      # Abstract base class
│   ├── Electronics.cs                  # Electronics with Brand, Warranty
│   └── Grocery.cs                      # Grocery with ExpiryDate, Organic
│
├── Services/
│   ├── IInventory.cs                   # Interface with method signatures
│   └── InventoryManager.cs             # Complete implementation
│
├── Exceptions/
│   └── InventoryExceptions.cs          # Custom exception classes
│
└── README.md                           # Project documentation
```

---

## 🔧 Installation & Setup

### Prerequisites

| Requirement | Version |
|-------------|---------|
| .NET SDK | 8.0 or higher |
| C# IDE | Visual Studio 2022 / VS Code |
| Git | Any version |

### Step-by-Step Installation

```bash
# 1. Clone the repository
git clone https://github.com/yourusername/advanced-inventory-system.git

# 2. Navigate into project directory
cd advanced-inventory-system

# 3. Restore dependencies
dotnet restore

# 4. Build the project
dotnet build

# 5. Run the application
dotnet run
```

### Visual Studio Setup
1. Open Visual Studio 2022
2. Click "Open a project or solution"
3. Select the `.csproj` file
4. Press `F5` to build and run

---

## 📺 How to Use

### Main Menu

```
╔═══════════════════════════════════════════════════════════════╗
║           ADVANCED INVENTORY MANAGEMENT SYSTEM                ║
╠═══════════════════════════════════════════════════════════════╣
║                                                               ║
║   📦 PRODUCT MANAGEMENT                                       ║
║      1. Add Electronics Product                               ║
║      2. Add Grocery Product                                   ║
║      3. View All Products                                     ║
║      4. Update Product                                        ║
║      5. Delete Product                                        ║
║                                                               ║
║   💰 SALES & STOCK                                            ║
║      6. Search Product                                        ║
║      7. Sell Product                                          ║
║      8. Total Inventory Value                                 ║
║                                                               ║
║   🚪 OTHER                                                    ║
║      9. Exit                                                  ║
║                                                               ║
╚═══════════════════════════════════════════════════════════════╝
```

### Usage Examples

**Add Electronics:**
```
Option 1 → ID: 101 → Name: iPhone 15 → Price: 999.99 → Qty: 10 → Brand: Apple → Warranty: 12 → Refurbished: n
```

**Add Grocery:**
```
Option 2 → ID: 201 → Name: Organic Milk → Price: 5.99 → Qty: 20 → Expiry: 2025-12-31 → Organic: y → Weight: 1
```

**Sell Product:**
```
Option 7 → ID: 101 → Qty: 2 → System shows total amount and updates stock
```

---

## 💻 Code Examples

### Abstract Base Class
```csharp
public abstract class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    
    public virtual string GetStockStatus()
    {
        if (Quantity <= 0) return "OUT OF STOCK";
        if (Quantity < 5) return "LOW STOCK";
        if (Quantity < 20) return "MEDIUM";
        return "HIGH";
    }
    
    public abstract void Display();
    public virtual double CalculateDiscount() => Price;
}
```

### Inheritance & Polymorphism
```csharp
public class Electronics : Product
{
    public string Brand { get; set; }
    public int WarrantyMonths { get; set; }
    public bool IsRefurbished { get; set; }
    
    public override double CalculateDiscount()
    {
        return IsRefurbished ? Price * 0.95 : Price * 0.90;
    }
    
    public override void Display()
    {
        Console.WriteLine($"[Electronics] {Name} | ${Price} | Qty:{Quantity} | {Brand}");
    }
}
```

### Event Delegate for Stock Alerts
```csharp
public delegate void StockAlertHandler(string message);
public event StockAlertHandler OnLowStock;

public void SellProduct(int id, int quantity)
{
    // ... sell logic
    if (product.Quantity < 5)
    {
        OnLowStock?.Invoke($"Low stock: {product.Name} has only {product.Quantity} left!");
    }
}
```

### LINQ Query Example
```csharp
// Search products by name
public List<Product> SearchProducts(string keyword)
{
    return _products.Where(p => 
        p.Name.ToLower().Contains(keyword.ToLower())).ToList();
}

// Calculate total inventory value
public double GetTotalInventoryValue()
{
    return _products.Sum(p => p.Price * p.Quantity);
}
```

---

## 📸 Sample Output

```
┌─────────────────────────────────────────┐
│ ID: 101  | New Electronics              │
│ Name: iPhone 15                         │
│ Price: $999.99 | Qty: 8                 │
│ Stock Status: MEDIUM                    │
│ Brand: Apple | Warranty: 12 months      │
│ Discounted Price: $899.99               │
└─────────────────────────────────────────┘

┌─────────────────────────────────────────┐
│ ID: 201  | Organic Grocery              │
│ Name: Organic Milk                      │
│ Price: $5.99 | Qty: 20                  │
│ Stock Status: HIGH                      │
│ Expiry: 12/31/2025 | Weight: 1kg        │
│ Discounted Price: $5.69                 │
└─────────────────────────────────────────┘
```

---

## 🔄 Future Enhancements

- [ ] Database integration (SQL Server)
- [ ] Web API version with ASP.NET Core
- [ ] GUI with Windows Forms / WPF
- [ ] User authentication (Admin/Staff roles)
- [ ] Export reports to PDF/Excel
- [ ] Barcode scanning support
- [ ] Dashboard with charts

---

## 👨‍💻 Author

**Your Name**
- GitHub: [@Umar-mustafa0177](https://github.com/Umar-mustafa0177)



---

## ⭐ Show Your Support

If this project helped you, please give it a star on GitHub!
