namespace AdForm_API.Models
{
    public class OrderInvoiceResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "";
        public List<OrderProduct> Products { get; set; }
        public float TotalPrice { get; set; }
    }
    public class OrderProduct
    {
        public string Name { get; set; } = "";
        public int Quantity { get; set; }
        public float Discount { get; set; }
        public float Price { get; set; }
    }
}
