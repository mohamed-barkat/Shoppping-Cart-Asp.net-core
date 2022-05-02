using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Contracts;
using Shop.Data.DataContext;
using Shop.Domain.Models;
using Shop.Domin.Models.Carts;
using Shop.UI.CartDataStore;
using Shop.UI.SessionHelpers;
using Shop.UI.ViewModel.Cart;

namespace Shop.UI.Controllers.Carts
{

    public class CartController : Controller
    {

        private readonly ICartItemRepository _cartItemRepo;
        private readonly ICartRepository _cartRepo;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _usermanger;
        private readonly IProductRepository _productRepo;
        private ICartDataBase _cartDataBaseStore;
        private ICartSessionData _cartSessionData;
        // private readonly ISession _session;
        public CartController(ICartDataBase cartDataBaseStore,
            UserManager<ApplicationUser> userManger, ICartRepository cartRepo, ICartItemRepository cartItemRepo,
            ApplicationDbContext context, IProductRepository productRepository, ICartSessionData cartSessionData)
        {
            _cartDataBaseStore = cartDataBaseStore;
            _cartItemRepo = cartItemRepo;
            _cartRepo = cartRepo;
            _context = context;
            _cartSessionData = cartSessionData;
            //_session = session;
            _usermanger = userManger;
            _productRepo = productRepository;

        }
        public IActionResult Index()
        {

            return View();
        }
        public async Task<Cartvm> getcartItems()
        {
            int cartId = 0;
            var user = await GetCurrentUser();


            if (user != null)
            {

                cartId = await _cartRepo.GetCartIdForUser(user);
                return await _cartDataBaseStore.GetCartItems(cartId);


            }
            else
            {


                return await _cartSessionData.GetCartItems(HttpContext.Session);
            }


        }

        [HttpPost]
        public async Task AddtoCart(int quantity, int productId)
        {
            int cartId;
            var user = await _usermanger.GetUserAsync(User);
            string userId;
            if (user != null)
            {
                userId = user.Id;
                cartId = await _cartRepo.GetCartIdForUser(user);

                await _cartDataBaseStore.AddItemToCart(quantity, productId, userId, cartId);
            }
            else
            {
                await _cartSessionData.AddToCart(HttpContext.Session, quantity, productId);

            }
        }




        [HttpPost]
        public async Task DeleteFromCart(int productId)
        {

            var user = await _usermanger.GetUserAsync(User);
            string userId;
            if (user != null)
            {
                userId = user.Id;
                await _cartDataBaseStore.DeleteFromCart(productId, userId);
            }
            else
            {
                await _cartSessionData.DeleteFromCart(HttpContext.Session, productId);
            }

        }
        public async Task<ApplicationUser> GetCurrentUser()
        {
            var user = await _usermanger.GetUserAsync(User);
            return user;
        }

        public async Task<int> CartCount()
        {
            var count = 0;
            Cartvm shopcart = new Cartvm();
            if (!User.Identity.IsAuthenticated)
            {
                shopcart = await _cartSessionData.GetCartItems(HttpContext.Session);
                if (shopcart == null)
                {
                    return 0;

                }
                else
                {
                    if (shopcart.CartItems != null)
                    {
                        foreach (var item in shopcart.CartItems)
                        {
                            count = count + item.Quantity;
                        }
                        return count;
                    }
                    else
                    {
                        return count;
                    }

                }

            }

            else
            {
                int cartid = await _cartRepo.GetCartIdForUser(await _usermanger.GetUserAsync(User));

                shopcart = await _cartDataBaseStore.GetCartItems(cartid);

                if (shopcart == null)
                {
                    return 0;
                }
                else
                {
                    if (shopcart.CartItems != null)
                    {
                        foreach (var item in shopcart.CartItems)
                        {
                            count = count + item.Quantity;
                        }
                        return count;
                    }
                    else
                    {
                        return count;
                    }

                }
            }
        }

    }
}
