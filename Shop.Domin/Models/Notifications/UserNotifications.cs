using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domin.Models.Notifications
{
    public class UserNotifications
    {


        [Key]
        public int Id { get; set; }
        public Notification Notification { get; set; }
        public int? NotificationId { get; set; }

        public string NotifierId { get; set; }

        public ApplicationUser Notifier { get; set; }

    }
}
