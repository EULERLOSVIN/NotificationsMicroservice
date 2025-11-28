using MassTransit;
using NotificationsMicroservice.Events;

namespace NotificationsMicroservice.Consumers;

public class TripCreatedConsumer : IConsumer<TripCreatedEvent>
{
    private readonly ILogger<TripCreatedConsumer> _logger;

    public TripCreatedConsumer(ILogger<TripCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<TripCreatedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation(">>> [NOTIFICACIÓN] Nuevo viaje creado: TripId={TripId}, Pasajero={PassengerId}, Tarifa={EstimatedFare:C}", 
            message.TripId, message.PassengerId, message.EstimatedFare);

        // Aquí iría la lógica real de envío de email/push
        return Task.CompletedTask;
    }
}
