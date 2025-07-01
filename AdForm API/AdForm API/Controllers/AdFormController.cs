using AdForm_API.AdFormDB;
using AdForm_API.Models;
using AdForm_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdForm_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdFormController : ControllerBase
    {
        private AdFormService _adFormService;
        public AdFormController(AdFormService adFormService)
        {
            _adFormService = adFormService;
        }
        [HttpGet]
        public IActionResult GetOrders([FromQuery] string[] orderIds)
        {
            GetOrdersResponse response = _adFormService.GetOrders(orderIds);
            return (Ok(new
            {
                Success = response.Success,
                Data = response.Details,
                Message = response.Message
            }));
        }
        [HttpGet]
        [Route("products")]
        public IActionResult GetProducts(string productName = "")
        {
            GetProductsResponse response = _adFormService.GetProducts(productName);
            return (Ok(new
            {
                Success = response.Success,
                Data = response.Products,
                Message = response.Message
            }));
        }
        [HttpGet]
        [Route("discountedProducts")]
        public IActionResult GetDiscountedProducts()
        {
            GetDiscountedProductsResponse response = _adFormService.GetDiscountedProducts();
            return (Ok(new
            {
                // Returns information about the data and additional information about the request to the Front-End
                Success = response.Success,
                Data = response.Products,
                Message = response.Message
            }));
        }
        [HttpGet]
        [Route("orderInvoice")]
        public IActionResult GetOrderInvoice(string orderId)
        {
            OrderInvoiceResponse response = _adFormService.GetOrderInvoice(orderId);
            return (Ok(new
            {
                Success = response.Success,
                Products = response.Products,
                TotalPrice = response.TotalPrice,
                Message = response.Message
            }));
        }
        [HttpPost]
        public IActionResult PostOrder([FromQuery]List<int> productIds, [FromQuery]List<int> prices, string orderId)
        {
            PostResponse response = _adFormService.PostOrder(productIds, prices, orderId);
            return (Ok(new
            {
                Success = response.Success,
                Message = response.Message
            }));
        }
        /*[HttpPost]
        [Route("product")]
        public IActionResult PostProduct(string name, decimal price)
        {
            PostResponse response = _adFormService.PostProduct(name, price);
            return (Ok(new
            {
                Success = response.Success,
                Message = response.Message
            }));
        }*/
        [HttpPost]
        [Route("discount")]
        public IActionResult PostDiscount(int productId, int minimumQuantity, float percentage)
        {
            PostResponse response = _adFormService.PostDiscount(productId, minimumQuantity, percentage);
            return (Ok(new
            {
                Success = response.Success,
                Message = response.Message
            }));
        }
    }
}
