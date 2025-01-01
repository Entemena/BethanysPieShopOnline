using BethanysPieShopOnline.Models;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopOnline.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCart _shoppingCart;

        public OrderController(IShoppingCart shoppingCart, IOrderRepository orderRepository)
        {
            _shoppingCart = shoppingCart;
            _orderRepository = orderRepository;
        }

        public IActionResult Checkout() // Invoked on GET 
        {
            return View();
        }
        [HttpPost]  // Forces invokation on POST
        public IActionResult Checkout(Order order)
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            if (_shoppingCart.ShoppingCartItems.Count == 0) 
            {
                ModelState.AddModelError("", "Your cart is empty, please add some delicious pies!");
            }
            if (ModelState.IsValid) 
            { 
                _orderRepository.CreateOrder(order);
                _shoppingCart.ClearCart();
                return RedirectToAction("CheckoutComplete");
            }

            return View(order);
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Thanks for your order, your pies will be with you soon!";
            return View();
        }
    }
}
