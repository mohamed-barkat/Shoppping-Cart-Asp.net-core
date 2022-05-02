using Microsoft.AspNetCore.Mvc;
using Shop.Domain.Models;
using Shop.UI.Utilities;
using Shop.UI.ViewModel.Address;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Shop.UI.ViewModel.Users
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<Claim>(); Roles = new List<string>();
        }
        public string Id { get; set; }

        [Required]
        [Remote(action: "IsUserInUse", controller: "Account")]
        [StructureUserNameAllowed(UserName: new string[] {
                "1","2","3","4","5","6","7","8","9","10","`","?","<","}"," ","  ","   ","12"
            }
)]
        public string UserName { get; set; }
        [Required]

        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "Account")]
        public string Email { get; set; }
        
        public string CraetedAt { get; set; }
        public List<Claim> Claims { get; set; }
        public IList<string> Roles { get; set; }
        public string PhoneNummber { get; set; }
        public bool IsEmailConfirmed { get; set; }

     
        [DataType(DataType.Password)]
        public string Password { get; set; }



    }
}
