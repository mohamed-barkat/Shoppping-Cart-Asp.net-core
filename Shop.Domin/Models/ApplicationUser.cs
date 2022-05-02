

using Microsoft.AspNetCore.Identity;
using Shop.Domin.Models.Carts;
using Shop.Domin.Models.Notifications;
using Shop.Domin.Models.Orders;

namespace Shop.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            CreatedAt = DateTime.Now;
        }


        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public Cart? Cart { get; set; }
        public ICollection<Order> Orders { get; set; }
        public List<Notification> Notifications { get; set; }

        public List<Notification> ActorNotifictions { get; set; }
        public List<UserNotifications> UserNotifications { get; set; }
    }
}
