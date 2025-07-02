using AdForm_API.AdFormDB;
using AdForm_API.Models;
using AdForm_API.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
        /// <summary>
        /// Get order details by the specified Order ids
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET/AdForm
        ///     {
        ///         "orderIds": ["ord1", "ord2"]
        ///     }
        /// </remarks>
        /// <param name="orderIds"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Provides a list of all of the available products or a select product if product name is provided
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET/AdForm/products
        ///     {
        ///         "productName": "Apples"
        ///     }
        /// </remarks>
        /// <param name="productName"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Shows all of the discounts product have that are in an order
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Get details about a specific order
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET/AdForm/orderInvoice
        ///     {
        ///         "orderId": "ord4"
        ///     }
        /// </remarks>
        /// <param name="orderId"></param>
        /// <response code="400">Missing order id</response>
        /// <returns></returns>
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
        /// <summary>
        /// Upload a new order
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST/AdForm
        ///     {
        ///         "orderId": "ord4",
        ///         "productIds": [1, 2],
        ///         "quantities": [10, 20]
        ///     }
        /// </remarks>
        /// <param name="productIds"></param>
        /// <param name="quantities"></param>
        /// <param name="orderId"></param>
        /// <response code="400">Missing order id</response>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PostOrder([FromQuery]List<int> productIds, [FromQuery]List<int> quantities, string orderId)
        {
            PostResponse response = _adFormService.PostOrder(productIds, quantities, orderId);
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
        /// <summary>
        /// Add a discount to a product
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST/AdForm/discount
        ///     {
        ///         "productId": 1,
        ///         "minimumQuantity": 10,
        ///         "percentage": 10
        ///     }
        /// </remarks>
        /// <param name="productId"></param>
        /// <param name="minimumQuantity"></param>
        /// <param name="percentage"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("discount")]
        public IActionResult PostDiscount([Required]int productId, [Required]int minimumQuantity, [Required]float percentage)
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
