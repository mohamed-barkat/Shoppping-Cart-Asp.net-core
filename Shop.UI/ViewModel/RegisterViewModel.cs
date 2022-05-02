using Microsoft.AspNetCore.Mvc;
using Shop.Domain.Models;
using Shop.UI.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Shop.UI.ViewModel
{
    public class RegisterViewModel
    {



        // [Remote(action: "IsEmailInUse",controller:"Account")]
        [Required]
        
        [EmailAddress]
         [Remote(action: "IsEmailInUse", controller:"Account")]
       // [ValidEmailDomain(EmailDomain: "gmail.com", ErrorMessage = "Email Domin must be gmail.com")]
        public string Email { get; set; }
        [Required]
       [Remote(action: "IsUserInUse", controller:"Account")]
       [StructureUserNameAllowed(UserName: new string[] {
                "1","2","3","4","5","6","7","8","9","10","`","?","<","}"," ","  ","   ","12"
            }
)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int PostalCode { get; set; }
    }
}
