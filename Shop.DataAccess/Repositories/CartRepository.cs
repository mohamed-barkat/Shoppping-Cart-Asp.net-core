using Microsoft.EntityFrameworkCore;
using Shop.Application.Contracts;
using Shop.Data.DataContext;
using Shop.Domain.Models;
using Shop.Domin.Models.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DataAccess.Repositories
{
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context)
        {

        }


        public async Task<int> GetCartIdForUser(ApplicationUser user)
        {

            var Cart = await _context.Carts.Where(c => c.UserId == user.Id).SingleOrDefaultAsync();
            return Cart.Id;
        }
    }
}
