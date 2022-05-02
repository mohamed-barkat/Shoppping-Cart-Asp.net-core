using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Contracts;
using Shop.Domain.Models;
using Shop.UI.EmailServices;
using Shop.UI.Models;
using Shop.UI.ViewModel.Cart;
using Shop.UI.ViewModel.Home;
using System.Diagnostics;

namespace Shop.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailService _email;
        private readonly IProductRepository _productRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartRepository _cart;
        public HomeController(ILogger<HomeController> logger, IEmailService email, IProductRepository productRepository, UserManager<ApplicationUser> userManger, ICartRepository cartRepository)
        {
            _logger = logger;
            _email = email;
            _productRepository = productRepository;
            _userManager = userManger;
            _cart = cartRepository;
        }

        public async Task<IActionResult> Index()
        {

            var user = await _userManager.GetUserAsync(User);
            int cartId;
            if (user != null)
            {
                cartId = await _cart.GetCartIdForUser(user);

            }
            else
            {
                cartId = 0;
            }


            var products = await _productRepository.GetAllAsync();
            var model = new CartItemsVm();
            var cartItems = new List<CartItemVM>();

            foreach (var product in products)
            {

                cartItems.Add(new CartItemVM()
                {
                    ProductId = product.Id,

                    Quantity = 1,

                    Name = product.Name,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    CreatedAt = product.CreatedAt.ToString(),
                });
            }
            model.CartId = cartId;
            model.cartItems = cartItems;
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}