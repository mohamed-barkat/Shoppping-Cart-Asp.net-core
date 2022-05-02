using System.ComponentModel.DataAnnotations;

namespace Shop.UI.ViewModel.Claims
{
    public class ClaimViewModel
    {

        public string Id { get; set; }
        [Required]
      
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public int UsersCount { get; set; }
    }
}
