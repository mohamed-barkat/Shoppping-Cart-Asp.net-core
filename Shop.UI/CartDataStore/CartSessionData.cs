using Shop.Application.Contracts;
using Shop.UI.SessionHelpers;
using Shop.UI.ViewModel.Cart;

namespace Shop.UI.CartDataStore
{
    public class CartSessionData : ICartSessionData
    {

        private readonly IProductRepository _productRepository;
        public CartSessionData(IProductRepository productRepository)
        {

            _productRepository = productRepository;
        }
        public async Task AddToCart(ISession _session, int quantity, int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            await _session.LoadAsync();
            quantity = quantity <= 0 ? 1 : quantity;
            var shopcart = SessionHelper.Get<Cartvm>(_session, "cart");
            decimal sum = 0;

            if (shopcart == null)
            {
                //shopcart.CartId = cartId;
                shopcart = new Cartvm()
                {
                    CartId = 0,
                    TotalPrice = (decimal)(quantity * product.Price),
                    CartItems = new List<CartItemVM> { new CartItemVM
                    {
                        ImageUrl =product.ImageUrl,
                        Price=product.Price,
                        Name=product.Name,
                        ProductId=productId,
                        Quantity=quantity,
                        Description=product.Description,
                        CreatedAt=product.CreatedAt.ToString(),

                    } }
                };

                await _session.LoadAsync();
                SessionHelper.Set<Cartvm>(_session, "cart", shopcart);


            }
            else
            {
                if (shopcart.CartItems.Any(ci => ci.ProductId == productId))
                {

                    shopcart.CartItems.SingleOrDefault(ci => ci.ProductId == productId).Quantity += quantity;
                    for (int i = 0; i < shopcart.CartItems.Count; i++)
                    {
                        sum = (decimal)(sum + shopcart.CartItems[i].Quantity * shopcart.CartItems[i].Price);
                    }
                    shopcart.TotalPrice = sum;

                }
                else
                {
                    shopcart.CartItems.Add(new CartItemVM
                    {
                        Quantity = quantity,
                        Name = product.Name,
                        ImageUrl = product.ImageUrl,
                        Price = product.Price,
                        ProductId = productId,
                        CreatedAt = product.CreatedAt.ToString(),
                        Description = product.Description,
                    });

                    for (int i = 0; i < shopcart.CartItems.Count; i++)
                    {
                        sum = (decimal)(sum + shopcart.CartItems[i].Quantity * shopcart.CartItems[i].Price);
                    }
                    shopcart.TotalPrice = sum;
                }
                await _session.LoadAsync();
                SessionHelper.Set<Cartvm>(_session, "cart", shopcart);
            }
        }

        public async Task DeleteFromCart(ISession session, int productId)
        {
            await session.LoadAsync();
            var shopcart = SessionHelper.Get<Cartvm>(session, "cart");

            var cartitem = shopcart.CartItems.SingleOrDefault(cr => cr.ProductId == productId);
            shopcart.CartItems.Remove(cartitem);

            SessionHelper.Set<Cartvm>(session, "cart", shopcart);
        }

        public async Task<Cartvm> GetCartItems(ISession session)
        {
            await session.LoadAsync();

            Cartvm shopcart = session.Get<Cartvm>("cart");


            if (shopcart == null)
            {
                shopcart = new Cartvm()
                {
                    CartId = 0,
                    CartItems = new List<CartItemVM>(),
                    TotalPrice = 0,

                };
            }

            return shopcart;
        }
    }
}
