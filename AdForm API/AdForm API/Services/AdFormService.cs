using AdForm_API.AdFormDB;
using AdForm_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AdForm_API.Services
{
    public class AdFormService
    {
        private AdFormContext _AdformContext;
        public AdFormService(AdFormContext AdformContext)
        {
            _AdformContext = AdformContext;
        }
        public GetProductsResponse GetProducts(string productName)
        {
            // A seperate object where all of the info can be added and then sent back
            GetProductsResponse response = new GetProductsResponse();
            response.Products = _AdformContext.Products.Where(x => productName == "" ? true : x.Name == productName).ToList();
            if (response.Products.Count() == 0) // If nothing was found
            {
                response.Success = false;
                response.Message = "Product: " + productName + " not found";
            }
            return response;
        }
        public GetOrdersResponse GetOrders(string[] orderIds)
        {
            // A seperate object where all of the info can be added and then sent back
            GetOrdersResponse response = new GetOrdersResponse();
            response.Details = (from o in _AdformContext.Orders
                                join p in _AdformContext.Products on o.ProductId equals p.ProductId
                                where orderIds.Contains(o.OrderId)
                                group new { o, p } by o.OrderId  into g
                                select new OrderDetails
                                {
                                    OrderId = g.Key,
                                    TotalPrice = g.Sum(x => x.o.Quantity * x.p.Price),
                                    Products = g.Count()
                                }).ToList();
            if (response.Details.Count() == 0)
            {
                // If nothing was found
                response.Success = false;
                response.Message = "No orders found";
            }
            return response;
        }
        public GetDiscountedProductsResponse GetDiscountedProducts()
        {
            // A seperate object where all of the info can be added and then sent back
            GetDiscountedProductsResponse response = new GetDiscountedProductsResponse();
            response.Products = (from d in _AdformContext.Discounts
                        join p in _AdformContext.Products on d.ProductId equals p.ProductId
                        join o in _AdformContext.Orders on new { d.ProductId } equals new { o.ProductId }
                        where d.MinQuantity <= o.Quantity // Join only if the ordered quantity exceeds or is equal to the minimum discount requirements
                        group new { d, p, o } by new { p.ProductId, p.Name, p.Price, d.Percentage } into g
                        select new DiscountedProduct
                        {
                            Name = g.Key.Name,
                            DiscountPercentage = g.Key.Percentage,
                            OrderCount = g.Select(x => x.o.OrderId).Distinct().Count(),
                            TotalQuantity = g.Sum(x => x.o.Quantity),
                            Price = g.Key.Price,
                            TotalPrice = g.Sum(x => x.o.Quantity) * g.Key.Price // Shown pricw without the discount. In case front end wants to the the math themselves and see accurate information
                        }).ToList();
            if (response.Products.Count() == 0)
            {
                // If nothing was found
                response.Success = false;
                response.Message = "No applicable discounts found";
            }
            return response;
        }
        public OrderInvoiceResponse GetOrderInvoice(string orderId)
        {
            // A seperate object where all of the info can be added and then sent back
            OrderInvoiceResponse response = new OrderInvoiceResponse();
            response.Products = (from o in _AdformContext.Orders
                        join p in _AdformContext.Products on o.ProductId equals p.ProductId
                        join d in _AdformContext.Discounts on o.ProductId equals d.ProductId
                        where o.OrderId == orderId
                        group new { o, p, d } by new { o.OrderId, p.Name, o.Quantity, d.Percentage, p.Price, d.MinQuantity } into g
                        select new OrderProduct {
                            Name = g.Key.Name,
                            Quantity = g.Key.Quantity,
                            Discount = g.Key.Quantity >= g.Key.MinQuantity ? g.Key.Percentage : 0, // If the discount requirements are met, then discount is shown.
                            Price = (float)g.Key.Price
                        }).ToList();
            if (response.Products.Count() == 0)
            {
                // If nothing was found
                response.Success = false;
                response.Message = "Order Id: " + orderId + " not found";
                return response;
            }
            // Total price (with discount) is only calculated after confirmation that an order was found
            foreach (OrderProduct product in response.Products)
            {
                float discount = product.Discount == 0 ? 1 : product.Discount / 100; // set discount to 100th of the number
                float price = product.Quantity * product.Price; // Calculate the discounted percentage from price
                response.TotalPrice = response.TotalPrice + (price - (price * discount)); // subtract the discounted price from the product real price
            }
            return response;
        }
        public PostResponse PostOrder(List<int> productIds, List<int> quantity, string orderId)
        {
            // A seperate object where all of the info can be added and then sent back
            PostResponse response = new PostResponse();
            if (productIds.Count() != quantity.Count)
            {
                // Each Product Id must have a quantity and a Quantity must have a Product Id
                response.Success = false;
                response.Message = "Product Ids and prices numbers do not match";
                return response;
            }
            for(int i = 0; i<productIds.Count(); i++) {
                Order order = new Order();
                order.OrderId = orderId;
                order.ProductId = productIds[i];
                order.Quantity = quantity[i];
                _AdformContext.Orders.Add(order);
            } 
            return SaveChanges(response);
        }
        public PostResponse PostProduct(string name, decimal price)
        {
            // A seperate object where all of the info can be added and then sent back
            PostResponse response = new PostResponse();
            Product product = new Product();
            product.Name = name;
            product.Price = price;
            _AdformContext.Products.Add(product);
            return SaveChanges(response);
        }
        public PostResponse PostDiscount(int productId, int minimumQuantity, float percentage)
        {
            // A seperate object where all of the info can be added and then sent back
            PostResponse response = new PostResponse();
            Discount discount = new Discount();
            discount.ProductId = productId;
            discount.Percentage = percentage;
            discount.MinQuantity = minimumQuantity;
            _AdformContext.Discounts.Add(discount);
            return SaveChanges(response);
        }
        public PostResponse SaveChanges(PostResponse response)
        {
            // A function to complete the upload and a message if something goes wrong
            try {
                _AdformContext.SaveChanges();
            } catch(Exception e){
                response.Success = false;
                response.Message = e.Message;
            }
            return response;
        }
    }
}
