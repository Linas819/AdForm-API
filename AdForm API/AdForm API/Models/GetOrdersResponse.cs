namespace AdForm_API.Models
{
    public class GetOrdersResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "";
        public List<OrderDetails> Details { get; set; }

    }
    public class OrderDetails
    {
        public string OrderId { get; set; } = "";
        public int Products { get; set; } = 0;
        public decimal TotalPrice { get; set; } = 0;
    }
}
