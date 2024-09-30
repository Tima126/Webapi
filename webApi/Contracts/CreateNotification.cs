namespace webApi.Contracts
{
    public class CreateNotification
    {
        public int NotificationId { get; set; }
        public string Message { get; set; } = null!;
        public DateTime? SentDate { get; set; }
    }
}
