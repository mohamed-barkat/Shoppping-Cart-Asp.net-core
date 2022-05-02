using Shop.Domain.Models;
using Shop.Domin.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domin.Models.Notifications
{
    public class Notification
    {


        public Notification()
        {
            this.CreateAt = DateTime.Now;
        }
        public int Id { get; set; }

        public string? Title { get; set; }
        public string? Body { get; set; }

        public int? Status { get; set; }
        public DateTime? ReadTime { get; set; }
        public DateTime CreateAt { get; set; }
        public bool? IsReaded { get; set; }

        public string? RedierectUrl { get; set; }
        public string? EntityId { get; set; }
        public EntityType? EntityTypeId { get; set; }
        public string? ActorId { get; set; }
        public ApplicationUser Actor { get; set; }

        public List<UserNotifications> UserNotifications { get; set; }





    }
}
