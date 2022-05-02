using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Shop.UI.ViewModel.Cart;

namespace Shop.UI.Controllers.Orders
{
    public class OrderController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        public OrderController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            List<Cartvm> m = new List<Cartvm>()
            {
               new Cartvm
               {
                   CartId = 1,
               }
            };

            if (!_memoryCache.TryGetValue("cart", out List<Cartvm> list))
            {
                _memoryCache.Set<List<Cartvm>>("cart", m, DateTimeOffset.Now.AddDays(2));
                return BadRequest();
            }


            return Ok(list);




        }


    }
}
