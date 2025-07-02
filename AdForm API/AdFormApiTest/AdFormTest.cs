using AdForm_API.AdFormDB;
using AdForm_API.Models;
using AdForm_API.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Fabric;

namespace AdFormApiTest
{
    public class AdFormTest
    {
        List<Order> orders = new List<Order>
            {
                new Order { Id = 1, OrderId = "ord1", ProductId = 1, Quantity = 20 },
                new Order { Id = 2, OrderId = "ord1", ProductId = 2, Quantity = 10 },
                new Order { Id = 3, OrderId = "ord2", ProductId = 1, Quantity = 10 }
            };
        List<Product> products = new List<Product>
            {
                new Product { ProductId = 1, Name = "Apples", Price = 1.12m },
                new Product { ProductId = 2, Name = "Oranges", Price = 1.25m }
            };
        List<Discount> discounts = new List<Discount>
        {
            new Discount { DiscountId = 1, ProductId = 1, Percentage = 20, MinQuantity = 5 }
        };
        public static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return mockSet;
        }
        [Fact]
        public void GetOrders() // Checks if data is retrieved
        {
            var mockOrderSet = CreateMockDbSet(orders);
            var mockProductSet = CreateMockDbSet(products);

            var mockContext = new Mock<AdFormContext>();
            mockContext.Setup(c => c.Orders).Returns(mockOrderSet.Object);
            mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            string[] orderIds = ["ord1", "ord2"];

            GetOrdersResponse result = service.GetOrders(orderIds);

            Assert.True(result.Success);
        }
        [Fact]
        public void GetOrdersNotFoundError() // Checks if Error message is correct
        {
            var mockOrderSet = CreateMockDbSet(orders);
            var mockProductSet = CreateMockDbSet(products);

            var mockContext = new Mock<AdFormContext>();
            mockContext.Setup(c => c.Orders).Returns(mockOrderSet.Object);
            mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            string[] orderIds = [];

            GetOrdersResponse result = service.GetOrders(orderIds);

            Assert.Equal("No orders found", result.Message );
        }
        [Fact]
        public void GetProducts() // Checks if products are retrieved
        {
            var mockProductSet = CreateMockDbSet(products);

            var mockContext = new Mock<AdFormContext>();
            mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            GetProductsResponse result = service.GetProducts("");

            Assert.True(result.Products.Count() > 1);
        }
        [Fact]
        public void GetProductByName() // Checks if products are retrieved by a specific name
        {
            var mockProductSet = CreateMockDbSet(products);

            var mockContext = new Mock<AdFormContext>();
            mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            GetProductsResponse result = service.GetProducts("Apples");

            Assert.Equal("Apples", result.Products[0].Name);
        }
        [Fact]
        public void GetProductNotFound() // Checks if error message is sent
        {
            var mockProductSet = CreateMockDbSet(products);

            var mockContext = new Mock<AdFormContext>();
            mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            GetProductsResponse result = service.GetProducts("Chair");

            Assert.Equal("Product: Chair not found", result.Message);
        }
        [Fact]
        public void GetDiscounts() // Checks if discounts are retrieved
        {
            var mockDiscountSet = CreateMockDbSet(discounts);
            var mockOrderSet = CreateMockDbSet(orders);
            var mockProductSet = CreateMockDbSet(products);

            var mockContext = new Mock<AdFormContext>();
            mockContext.Setup(c => c.Discounts).Returns(mockDiscountSet.Object);
            mockContext.Setup(c => c.Orders).Returns(mockOrderSet.Object);
            mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            GetDiscountedProductsResponse result = service.GetDiscountedProducts();

            Assert.True(result.Success);
        }
        [Fact]
        public void GetOrderInvoice() // Checks if discounts are retrieved
        {
            var mockDiscountSet = CreateMockDbSet(discounts);
            var mockOrderSet = CreateMockDbSet(orders);
            var mockProductSet = CreateMockDbSet(products);

            var mockContext = new Mock<AdFormContext>();
            mockContext.Setup(c => c.Discounts).Returns(mockDiscountSet.Object);
            mockContext.Setup(c => c.Orders).Returns(mockOrderSet.Object);
            mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            OrderInvoiceResponse result = service.GetOrderInvoice("ord1");

            Assert.True(result.Products.Count() == 2);
        }
        [Fact]
        public void GetOrderInvoiceTotalPrice() // Checks if total price is correct
        {
            var mockDiscountSet = CreateMockDbSet(discounts);
            var mockOrderSet = CreateMockDbSet(orders);
            var mockProductSet = CreateMockDbSet(products);

            var mockContext = new Mock<AdFormContext>();
            mockContext.Setup(c => c.Discounts).Returns(mockDiscountSet.Object);
            mockContext.Setup(c => c.Orders).Returns(mockOrderSet.Object);
            mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            OrderInvoiceResponse result = service.GetOrderInvoice("ord1");

            Assert.Equal(17.92m, (decimal)result.TotalPrice);
        }
        [Fact]
        public void GetOrderInvoiceNotFound() // Checks order not found error messege
        {
            var mockDiscountSet = CreateMockDbSet(discounts);
            var mockOrderSet = CreateMockDbSet(orders);
            var mockProductSet = CreateMockDbSet(products);

            var mockContext = new Mock<AdFormContext>();
            mockContext.Setup(c => c.Discounts).Returns(mockDiscountSet.Object);
            mockContext.Setup(c => c.Orders).Returns(mockOrderSet.Object);
            mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            OrderInvoiceResponse result = service.GetOrderInvoice("ord");

            Assert.Equal("Order Id: ord not found", result.Message);
        }
        [Fact]
        public void PostOrder()
        {
            var mockOrderSet = CreateMockDbSet(orders);

            var mockContext = new Mock<AdFormContext>();
            mockContext.Setup(c => c.Orders).Returns(mockOrderSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            List<int> productIds = new List<int> { 1, 2 };
            List<int> quantities = new List<int> { 20, 30 };

            PostResponse result = service.PostOrder(productIds, quantities, "ord3");

            Assert.True(result.Success);
        }
        [Fact]
        public void PostOrderErrorMessege() // Checks if error messege is recieved
        {
            var mockOrderSet = CreateMockDbSet(orders);

            var mockContext = new Mock<AdFormContext>();
            mockContext.Setup(c => c.Orders).Returns(mockOrderSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            List<int> productIds = new List<int> { 1, 2 };
            List<int> quantities = new List<int> { 20 };

            PostResponse result = service.PostOrder(productIds, quantities, "ord3");

            Assert.Equal("Product Ids and prices numbers do not match", result.Message);
        }
        [Fact]
        public void PostOrderInvalidEntries() // Checks if error messege is recieved for invalid entries
        {
            var mockOrderSet = CreateMockDbSet(orders);

            var mockContext = new Mock<AdFormContext>();
            mockContext.Setup(c => c.Orders).Returns(mockOrderSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            List<int> productIds = new List<int> { -1, 2 };
            List<int> quantities = new List<int> { 20, 15 };

            PostResponse result = service.PostOrder(productIds, quantities, "ord3");

            Assert.Equal("ProductId and/or Qauntity cannot be lower or at 0", result.Message);
        }
        [Fact]
        public void PostDiscount() // Checks if a discount can be posted
        {
            var mockDiscountSet = CreateMockDbSet(discounts);

            var mockContext = new Mock<AdFormContext>();
            mockContext.Setup(c => c.Discounts).Returns(mockDiscountSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            PostResponse result = service.PostDiscount(2, 5, 13);

            Assert.True(result.Success);
        }
        [Fact]
        public void PostDiscountErrorMessege() // Checks if error messege is recieved
        {
            var mockDiscountSet = CreateMockDbSet(discounts);

            var mockContext = new Mock<AdFormContext>();
            mockContext.Setup(c => c.Discounts).Returns(mockDiscountSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            PostResponse result = service.PostDiscount(2, -5, 13);

            Assert.Equal("Minimum Quantity and/or Percentage cannot be lower , or at 0", result.Message);
        }
        [Fact]
        public void PostProduct()
        {
            var mockProductSet = CreateMockDbSet(products);
            var mockContext = new Mock<AdFormContext>();
            mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            PostResponse result = service.PostProduct("Toy", 1.45m);

            Assert.True(result.Success);
        }
        [Fact]
        public void PostProductNoName() // Checks if error is shown when no name is given
        {
            var mockProductSet = CreateMockDbSet(products);
            var mockContext = new Mock<AdFormContext>();
            mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            PostResponse result = service.PostProduct("", 1.45m);

            Assert.Equal("Product name required", result.Message);
        }
        [Fact]
        public void PostProductInvalidPrice() // Checks if error is shown when price is invalid
        {
            var mockProductSet = CreateMockDbSet(products);
            var mockContext = new Mock<AdFormContext>();
            mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            PostResponse result = service.PostProduct("Toy", -1.45m);

            Assert.Equal("Price cannot be lower, or at 0", result.Message);
        }
    }
}