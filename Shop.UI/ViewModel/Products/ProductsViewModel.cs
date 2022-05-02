using System.ComponentModel.DataAnnotations;

namespace Shop.UI.ViewModel.Products
{
    public class ProductsViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public bool IsEdited { get; set; }
        public decimal? Price { get; set; }
        public string CreatedAt { get; set; }

        public List<string> CategoriesNames { get; set; }
    }
}
