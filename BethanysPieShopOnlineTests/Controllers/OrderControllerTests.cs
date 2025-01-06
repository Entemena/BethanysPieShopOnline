using BethanysPieShopOnline.Controllers;
using BethanysPieShopOnline.Models;
using BethanysPieShopOnline.ViewModels;
using BethanysPieShopOnlineTests.Mocks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BethanysPieShopOnlineTests.Controllers
{
    public class OrderControllerTests
    {
        private Mock<IOrderRepository> _mockOrderRepository;
        private Mock<IShoppingCart> _mockShoppingCart;
        private OrderController _orderController;

        public OrderControllerTests()
        {
            // Initialize mocks
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockShoppingCart = new Mock<IShoppingCart>();

            // Initialize the controller with mocked dependencies
            _orderController = new OrderController(_mockShoppingCart.Object, _mockOrderRepository.Object);
        }

        [Fact]
        public void Checkout_ValidOrder_RedirectsToOrderComplete()
        {
            // Arrange
            var cartItems = new List<ShoppingCartItem>
    {
        new ShoppingCartItem { Pie = new Pie { PieId = 1, Name = "Apple Pie", Price = 10.99M }, Amount = 2 }
    };

            // Mock GetShoppingCartItems
            _mockShoppingCart.Setup(c => c.GetShoppingCartItems()).Returns(cartItems);

            // Mock ShoppingCartItems property
            _mockShoppingCart.SetupProperty(c => c.ShoppingCartItems, cartItems);

            // Mock GetShoppingCartTotal
            _mockShoppingCart.Setup(c => c.GetShoppingCartTotal()).Returns(21.98M);

            var order = new Order
            {
                FirstName = "John",
                LastName = "Doe",
                AddressLine1 = "123 Main St",
                City = "Test City",
                State = "TS",
                PostalCode = "12345",
                Country = "Test Country",
                PhoneNumber = "1234567890",
                EmailAddress = "john.doe@example.com",
                OrderTotal = 21.98M,
                OrderPlaced = DateTime.Now
            };

            // Act
            var result = _orderController.Checkout(order);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("CheckoutComplete", redirectResult.ActionName); // Updated to "CheckoutComplete"
            _mockOrderRepository.Verify(r => r.CreateOrder(It.IsAny<Order>()), Times.Once);
        }


        [Fact]
        public void Checkout_EmptyCart_ReturnsViewWithError()
        {
            // Arrange: Ensure the shopping cart has no items (empty)
            var cartItems = new List<ShoppingCartItem>();

            _mockShoppingCart.Setup(c => c.GetShoppingCartItems()).Returns(cartItems);
            _mockShoppingCart.Setup(c => c.GetShoppingCartTotal()).Returns(0M);
            _mockShoppingCart.SetupProperty(c => c.ShoppingCartItems, cartItems); // Mock ShoppingCartItems

            var order = new Order
            {
                FirstName = "John",
                LastName = "Doe",
                AddressLine1 = "123 Main St",
                City = "Test City",
                State = "TS",
                PostalCode = "12345",
                Country = "Test Country",
                PhoneNumber = "1234567890",
                EmailAddress = "john.doe@example.com"
            };

            // Act
            var result = _orderController.Checkout(order);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.False(viewResult.ViewData.ModelState.IsValid);
        }


        [Fact]
        public void Checkout_NonEmptyCart_ProcessesOrderSuccessfully()
        {
            // Arrange: Add an item to the shopping cart
            var cartItems = new List<ShoppingCartItem>
            {
                new ShoppingCartItem
                {
                    Pie = new Pie
                    {
                        PieId = 1,
                        Name = "Apple Pie",
                        Price = 10.99M
                    },
                    Amount = 2
                }
            };

            _mockShoppingCart.Setup(c => c.GetShoppingCartItems()).Returns(cartItems);
            _mockShoppingCart.Setup(c => c.GetShoppingCartTotal()).Returns(21.98M);
            _mockShoppingCart.SetupProperty(c => c.ShoppingCartItems, cartItems); // Mock ShoppingCartItems

            var order = new Order
            {
                FirstName = "Jane",
                LastName = "Smith",
                AddressLine1 = "456 Another St",
                City = "Another City",
                State = "AS",
                PostalCode = "54321",
                Country = "Another Country",
                PhoneNumber = "9876543210",
                EmailAddress = "jane.smith@example.com",
                OrderTotal = 21.98M
            };

            // Act
            var result = _orderController.Checkout(order);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("CheckoutComplete", redirectResult.ActionName);
            _mockOrderRepository.Verify(r => r.CreateOrder(It.IsAny<Order>()), Times.Once);
        }



    }

}
