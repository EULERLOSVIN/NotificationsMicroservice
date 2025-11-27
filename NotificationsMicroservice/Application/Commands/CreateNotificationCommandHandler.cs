using MediatR;
using NotificationsMicroservice.DTOs;
using NotificationsMicroservice.Services; // Tu Bus simulado

namespace NotificationsMicroservice.Features.Commands
{
    // 2. El Handler: Contiene la lógica de CÓMO procesar el comando.
    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, string>
    {
        private readonly NotificationListener _eventBus;

        // Inyectamos el Bus que creamos en el paso anterior
        public CreateNotificationCommandHandler(NotificationListener eventBus)
        {
            _eventBus = eventBus;
        }

        public Task<string> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            // Convertimos el Command (CQRS) al Evento (Bus)
            var notificationEvent = new NotificationEvent
            {
                AccountId = request.AccountId,
                RecipientContact = request.RecipientContact,
                Channel = request.Channel,
                Subject = request.Subject,
                Body = request.Body
            };

            // Publicamos al Bus (Fire and Forget)
            _eventBus.PublishMessage(notificationEvent);

            return Task.FromResult("Notificación enviada a la cola de procesamiento exitosamente.");
        }
    }
}