using DotLiquid.Tags;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Contracts;
using Shop.Data.DataContext;
using Shop.Domin.Models.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DataAccess.Repositories
{
    public class CartItemRepository : BaseRepository<CartItem>, ICartItemRepository
    {

        public CartItemRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<List<CartItem>> GetCartItemForUser(int cartId)
        {
            //var cartItems = await _context.Carts.Where(c => c.UserId == userId && c.Id == cartId)
            //     .Include(c => c.CartItems).ThenInclude(x => x.Product).Select(c => c.CartItems).ToListAsync();

            var cartItems = await _context.Set<CartItem>().Where(x => x.CartId == cartId).Include(x => x.Product).ToListAsync();

            return cartItems;
        }

        public async Task<CartItem> GetCartItemforUserByProductId(int productId, string userId)
        {


            var cartitem = await _context.Carts.Where(c => c.UserId == userId).Select(c => c.CartItems.Single(s => s.ProductId == productId))
                .SingleOrDefaultAsync();


            return cartitem;
        }




    }
}
