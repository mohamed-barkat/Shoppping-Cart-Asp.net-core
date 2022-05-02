using Shop.Domin.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contracts.Notifications
{
    public interface INotification : IAsyncRepository<Notification>
    {

        public Task<List<Notification>> GetUserNotifications(string userId);
    }
}
