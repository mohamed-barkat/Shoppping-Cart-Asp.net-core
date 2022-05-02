using Shop.Domain.Models;
using Shop.Domin.Models.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contracts
{
    public interface ICartItemRepository : IAsyncRepository<CartItem>
    {


        public Task<CartItem> GetCartItemforUserByProductId(int productId, string userId);
        public Task<List<CartItem>> GetCartItemForUser( int cartId);


    }
}
