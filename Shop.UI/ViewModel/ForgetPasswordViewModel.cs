using System.ComponentModel.DataAnnotations;

namespace Shop.UI.ViewModel
{
    public class ForgetPasswordViewModel
    {
       
        
            [EmailAddress]
            [Required]
            public string Email { get; set; }
      }
    
}
