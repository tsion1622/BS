namespace BM.Models
{
    public class SendNotificationRequest
    {
        public string Message { get; set; }

        public int NotificationTypeId { get; set; }
        public int NotificationStatusId { get; set; }
    }
}