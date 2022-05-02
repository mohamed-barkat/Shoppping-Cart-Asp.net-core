using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Shop.UI.ViewModel.Roles
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required]
        [Remote(action: "IsRoleInUser", controller: "Administration")]
        public string RoleName { get; set; }
      
        public int UsersCount { get; set; }
      

    }
}
