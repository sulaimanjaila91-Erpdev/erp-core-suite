// ErpCoreSuite.Core/Entities/Product.cs
namespace ErpCoreSuite.Core.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int StockQty { get; set; }
        public int ReorderLevel { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class StockTransaction
    {
        public int TransactionId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string TransactionType { get; set; }  // IN / OUT
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public string Reference { get; set; }
        public string Remarks { get; set; }
        public DateTime TransactionDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
