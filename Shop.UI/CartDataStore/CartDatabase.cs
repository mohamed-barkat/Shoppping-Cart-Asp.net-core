using Shop.Application.Contracts;
using Shop.Domin.Models.Carts;
using Shop.UI.ViewModel.Cart;

namespace Shop.UI.CartDataStore
{
    public class CartDatabase : ICartDataBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICartItemRepository _cartItemRepository;

        public CartDatabase(IProductRepository productRepository, ICartItemRepository cartItemRepository)
        {
            _productRepository = productRepository;
            _cartItemRepository = cartItemRepository;
        }
        public async Task AddItemToCart(
          int quantity, int productId, string userId, int cartId)
        {
            var product = await _productRepository.GetByIdAsync(productId);


            quantity = quantity <= 0 ? 1 : quantity;
            var cartItem = new CartItem()
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = quantity,


            };
            var cartitemd = await _cartItemRepository.GetCartItemforUserByProductId(productId, userId);
            if (cartitemd != null)
            {
                cartitemd.Quantity += quantity;
                await _cartItemRepository.UpdateAsync(cartitemd);
            }
            else
            {
                await _cartItemRepository.AddAsync(cartItem);
            }
        }

        public async Task DeleteFromCart(int productId, string userId)
        {
            var cartitem = await _cartItemRepository.GetCartItemforUserByProductId(productId, userId);

            await _cartItemRepository.DeleteAsync(cartitem);
        }

        public async Task<Cartvm> GetCartItems(int cartId)
        {
            decimal sum = 0;
            var cartvm = new Cartvm();
            var cartitems = await _cartItemRepository.GetCartItemForUser(cartId);
            cartvm.CartId = cartId;
            var cartitesmsVm = new List<CartItemVM>();
            if (cartitems == null)
            {
                cartvm.CartItems = new List<CartItemVM>();
                cartvm.TotalPrice = 0;
                return cartvm;
            }
            else
            {
                foreach (var item in cartitems)
                {


                    cartitesmsVm.Add(new CartItemVM()
                    {
                        CreatedAt = item.Product.CreatedAt.ToString(),
                        ImageUrl = item.Product.ImageUrl,
                        Description = item.Product.Description,
                        Name = item.Product.Name,
                        Price = item.Product.Price,
                        ProductId = (int)item.ProductId,
                        Quantity = (int)item.Quantity,

                    });
                    sum = (decimal)(sum + item.Product.Price * item.Quantity);
                }

                cartvm.CartItems = cartitesmsVm;

                cartvm.TotalPrice = sum;

                return cartvm;
            }



        }
    }
}

