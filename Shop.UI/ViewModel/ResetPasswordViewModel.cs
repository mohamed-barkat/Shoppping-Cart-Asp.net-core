using System.ComponentModel.DataAnnotations;

namespace Shop.UI.ViewModel
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessage = "Password and ConfirmPassword must be match")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
