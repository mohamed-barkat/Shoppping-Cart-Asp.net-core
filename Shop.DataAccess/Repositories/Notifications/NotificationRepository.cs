using Microsoft.EntityFrameworkCore;
using Shop.Application.Contracts.Notifications;
using Shop.Data.DataContext;
using Shop.Domin.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DataAccess.Repositories.Notifications
{
    public class NotificationRepository : BaseRepository<Notification>, INotification
    {

        public NotificationRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<List<Notification>> GetUserNotifications(string userId)
        {

            var nots = await _context.Notifications.Where(s => s.UserNotifications.Any(x => x.NotifierId == userId))
                .OrderByDescending(x=>x.CreateAt).ToListAsync();
            return nots;
        }

        //public async Task<List<Notification>> GetUserNotifications(string userId)
        //{
        //    var Notifications = await _context.Notifications.Where(n => n.NotifierId == userId).ToListAsync();
        //    return Notifications;
        //}
    }
}
