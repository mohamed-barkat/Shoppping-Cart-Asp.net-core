namespace Shop.UI.ViewModel.Cart
{
    public class CartItemVM
    {
        public int ProductId { get; set; }
      
        public string Name { get; set; }

        public string ImageUrl { get; set; }

       
        public decimal? Price { get; set; }
       

        public string Description { get; set; }
      
      
        public int Quantity { get; set; }
        public string CreatedAt { get; internal set; }
    }
}
