using MediatR;
using NotificationsMicroservice.DTOs;

namespace NotificationsMicroservice.Features.Commands
{
    // 1. El Comando: Define QUÉ datos necesito para ejecutar la acción.
    // Devuelve un string (o un bool) indicando si se encoló bien.
    public class CreateNotificationCommand : IRequest<string>
    {
        public int AccountId { get; set; }
        public string RecipientContact { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}