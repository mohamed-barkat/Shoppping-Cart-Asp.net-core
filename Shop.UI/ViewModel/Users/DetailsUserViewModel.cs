using Microsoft.AspNetCore.Mvc;
using Shop.Domain.Models;
using Shop.UI.ViewModel.Address;
using System.Security.Claims;

namespace Shop.UI.ViewModel.Users
{
    public class DetailsUserViewModel
    {
        public DetailsUserViewModel()
        {

            Claims = new List<Claim>(); Roles = new List<string>();
        }
        public string Id { get; set; }
        public int CartId { get; set; }
        public string UserName { get; set; }
      
        public string Email { get; set; }
        
        public string CraetedAt { get; set; }
        public List<Claim> Claims { get; set; }
        public IList<string> Roles { get; set; }
        public string PhoneNummber { get; set; }
        public bool IsEmailConfirmed { get; set; }

        public AddressViewModel address { get; set; }
    }
}
