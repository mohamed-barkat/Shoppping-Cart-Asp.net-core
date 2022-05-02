using System.ComponentModel.DataAnnotations;

namespace Shop.UI.ViewModel.Categories
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public int ProductCount { get; set; }
        public string CraetedAt { get; set; }
        public string? UpdatedAt { get; set; }
        public bool IsUpdated { get; set; }
    }
}
