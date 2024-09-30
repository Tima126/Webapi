namespace webApi.Contracts
{
    public class CreateNotificationType
    {
        public int NotificationTypeId { get; set; }

        public string TypeName { get; set; } = null!;
    }
}
