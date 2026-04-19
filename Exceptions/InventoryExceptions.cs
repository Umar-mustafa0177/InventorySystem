using System;

namespace InventorySystem.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(string message) : base(message) { }
    }

    public class InvalidQuantityException : Exception
    {
        public InvalidQuantityException(string message) : base(message) { }
    }

    public class InsufficientStockException : Exception
    {
        public InsufficientStockException(string message) : base(message) { }
    }

    public class ExpiredProductException : Exception
    {
        public ExpiredProductException(string message) : base(message) { }
    }
}