namespace NotificationsMicroservice.DTOs
{
    // Esto representa el mensaje que viaja por el "Bus"
    public class NotificationEvent
    {
        public int AccountId { get; set; }
        public string RecipientContact { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}