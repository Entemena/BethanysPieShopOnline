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
            var mockShoppingCart = RepositoryMocks.GetShoppingCart();
            var mockOrderRepository = RepositoryMocks.GetOrderRepository();

            var orderController = new OrderController(mockShoppingCart.Object, mockOrderRepository.Object);

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
                OrderTotal = mockShoppingCart.Object.GetShoppingCartTotal()
            };

            // Act
            var result = orderController.Checkout(order);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("CheckoutComplete", redirectResult.ActionName);
            mockOrderRepository.Verify(repo => repo.CreateOrder(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public void Checkout_EmptyCart_ReturnsViewWithError()
        {
            // Arrange: Mock the shopping cart with no items
            var mockShoppingCart = RepositoryMocks.GetShoppingCart();
            mockShoppingCart.Setup(cart => cart.GetShoppingCartItems()).Returns(new List<ShoppingCartItem>());
            mockShoppingCart.Setup(cart => cart.GetShoppingCartTotal()).Returns(0M);
            mockShoppingCart.SetupProperty(cart => cart.ShoppingCartItems, new List<ShoppingCartItem>());

            var mockOrderRepository = RepositoryMocks.GetOrderRepository();
            var orderController = new OrderController(mockShoppingCart.Object, mockOrderRepository.Object);

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
            var result = orderController.Checkout(order);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.False(viewResult.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Checkout_NonEmptyCart_ProcessesOrderSuccessfully()
        {
            // Arrange: Mock the shopping cart with items
            var mockShoppingCart = RepositoryMocks.GetShoppingCart();
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
            mockShoppingCart.Setup(cart => cart.GetShoppingCartItems()).Returns(cartItems);
            mockShoppingCart.Setup(cart => cart.GetShoppingCartTotal()).Returns(21.98M);
            mockShoppingCart.SetupProperty(cart => cart.ShoppingCartItems, cartItems);

            var mockOrderRepository = RepositoryMocks.GetOrderRepository();
            var orderController = new OrderController(mockShoppingCart.Object, mockOrderRepository.Object);

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
            var result = orderController.Checkout(order);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("CheckoutComplete", redirectResult.ActionName);
            mockOrderRepository.Verify(repo => repo.CreateOrder(It.IsAny<Order>()), Times.Once);
        }



    }

}
