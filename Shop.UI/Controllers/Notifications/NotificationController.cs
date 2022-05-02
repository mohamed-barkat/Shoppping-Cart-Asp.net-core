using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Contracts.Notifications;
using Shop.Data.DataContext;
using Shop.Domain.Models;
using Shop.Domin.Models.Notifications;
using Shop.UI.ViewModel.Notifications;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shop.UI.Controllers.Notifications
{
    public class NotificationController : Controller
    {
        private readonly INotification _notification;
        private readonly UserManager<ApplicationUser> _userManger;
       
        public NotificationController( INotification notification, UserManager<ApplicationUser> userManger)
        {
            _notification = notification;
            _userManger = userManger;
       
        }
        public IActionResult Index()
        {
            return View();
        }


        public async Task<List<NotificationViewModel>> GetUserNotifications()
        {
            var userName = User.Identity.Name;
            var userId = await GetcurrentUserId(userName);
            var notifications = new List<NotificationViewModel>();

            if (userId == null)
            {
                return notifications;
            }
            else
            {

                var nots = await _notification.GetUserNotifications(userId);
                if (nots != null)
                {
                    foreach (var notification in nots)
                    {
                        notifications.Add(new NotificationViewModel
                        {
                            CreatedAt = notification.CreateAt.ToString(),
                            Body = notification.Body,
                            IsReaded = notification.IsReaded,
                            ReadTime = notification.ReadTime.ToString(),
                            Id = notification.Id,
                            RedierectUrl = notification.RedierectUrl,
                            Status = notification.Status,
                            Title = notification.Title,


                        });
                    }

                }



                return notifications;



            }









        }







        [HttpPost]
        public async Task UpdatedUserNotifcaions()
        {
            var userId = await GetcurrentUserId(User.Identity.Name);

            var userNotifcaions = await _notification.GetUserNotifications(userId);
            if (userNotifcaions != null)
            {
                userNotifcaions[0].IsReaded = true;
                userNotifcaions[0].ReadTime = DateTime.Now;
                await _notification.UpdateAsync(userNotifcaions[0]);


            }


        }
        private async Task<string> GetcurrentUserId(string userName)
        {
            var user = await _userManger.GetUserAsync(User);

            if (user == null)
            {
                return null;
            }

            return user.Id;

        }
    }
}
