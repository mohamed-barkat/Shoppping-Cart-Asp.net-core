namespace Shop.UI.ViewModel.Notifications
{
    public class NotificationViewModel
    {

        public int Id { get; set; }

        public string? Title { get; set; }
        public string? Body { get; set; }

        public int? Status { get; set; }
        public string? ReadTime { get; set; }
        public string CreatedAt { get; set; }
        public bool? IsReaded { get; set; }

        public string? RedierectUrl { get; set; }


    }
}
