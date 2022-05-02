using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace Shop.UI.ViewModel
{
    public class LoginViewModel
    {


        // [Remote(action: "IsEmailInUse",controller:"Account")]
      
        [Required]
        //[Remote(action: "IsUserInUse", controller:"Account")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public bool RememberMe { get; set; }

        public string returnUrl { get; set; }

    }
}
