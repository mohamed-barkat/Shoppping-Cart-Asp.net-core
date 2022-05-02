using Shop.UI.ViewModel.Cart;

namespace Shop.UI.CartDataStore
{
    public interface ICartSessionData
    {
        public Task AddToCart(ISession session, int quantity, int productId);
        public Task DeleteFromCart(ISession session, int productId);
        public Task<Cartvm> GetCartItems(ISession session);
    }
}
