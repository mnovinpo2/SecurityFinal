public class MyProductsViewModel
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderItemDetails> Items { get; set; } = new List<OrderItemDetails>();
}

public class OrderItemDetails
{
    public int ProductId { get; set; } // Add this property
    public string ProductName { get; set; }
    public decimal ItemPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => Quantity * (ItemPrice - DiscountAmount);
}