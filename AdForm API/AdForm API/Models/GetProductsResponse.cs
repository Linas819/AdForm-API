using AdForm_API.AdFormDB;

namespace AdForm_API.Models
{
    public class GetProductsResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "";
        public List<Product> Products { get; set; }
    }
}
