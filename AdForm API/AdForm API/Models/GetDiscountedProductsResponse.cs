using AdForm_API.AdFormDB;

namespace AdForm_API.Models
{
    public class GetDiscountedProductsResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "";
        public List<DiscountedProduct> Products { get; set; }
}
    public class DiscountedProduct
    {
        public string Name { get; set; } = "";
        public float DiscountPercentage { get; set; }
        public int OrderCount { get; set; }
        public int TotalQuantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
