using Shop.UI.Paginated;
using Shop.UI.ViewModel.Categories;
using System.ComponentModel.DataAnnotations;

namespace Shop.UI.ViewModel.Products
{
    public class EditProductViewModel
    {
        [Required]
        public string Name { get; set; }
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
    
        public List<IFormFile> Files { get; set; }



    }
}
