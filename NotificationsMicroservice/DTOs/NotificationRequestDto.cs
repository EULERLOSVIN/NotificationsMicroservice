namespace NotificationsMicroservice.DTOs
{
    public class NotificationRequestDto
    {
        public int AccountId { get; set; }        // ID del usuario (AuthDB)
        public string RecipientContact { get; set; } = string.Empty; // Email o Celular
        public string Channel { get; set; } = "Email"; // 'Email', 'SMS', 'Push'
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}