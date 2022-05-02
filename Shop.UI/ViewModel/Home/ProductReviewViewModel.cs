namespace Shop.UI.ViewModel.Home
{
    public class ProductReviewViewModel
    {

        public int Id { get; set; }


        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public decimal? Price { get; set; }
        public string CreatedAt { get; set; }
        public List<string> CategoriesNames { get; set; }
    }
}
