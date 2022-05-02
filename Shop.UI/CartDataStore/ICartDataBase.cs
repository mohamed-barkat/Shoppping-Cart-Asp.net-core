using Shop.Application.Contracts;
using Shop.UI.ViewModel.Cart;

namespace Shop.UI.CartDataStore
{
    public interface ICartDataBase
    {

        public Task AddItemToCart(int quantity, int productId, string userId, int cartId);
        public Task DeleteFromCart(int productId, string userId);
        public Task<Cartvm> GetCartItems(int cartId);
    }
}
