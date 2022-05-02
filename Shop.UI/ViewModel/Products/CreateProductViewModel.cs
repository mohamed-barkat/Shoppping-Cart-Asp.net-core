using Shop.UI.Paginated;
using Shop.UI.ViewModel.Categories;
using System.ComponentModel.DataAnnotations;

namespace Shop.UI.ViewModel.Products
{
    public class CreateProductViewModel
    {

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public List<IFormFile> Files { get; set; }

        public decimal? Price { get; set; }
        public int categoryId { get; set; }
    }
}
