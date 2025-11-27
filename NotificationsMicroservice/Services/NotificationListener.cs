using System.Collections.Concurrent;
using NotificationsMicroservice.Data;
using NotificationsMicroservice.DTOs;
using NotificationsMicroservice.Entities;
using Microsoft.Extensions.Hosting; // Necesario para BackgroundService
using Microsoft.Extensions.DependencyInjection; // Necesario para IServiceScopeFactory

namespace NotificationsMicroservice.Services
{
    public class NotificationListener : BackgroundService
    {
        // Cola en memoria (Simula RabbitMQ)
        private static readonly ConcurrentQueue<NotificationEvent> _messageQueue = new();
        private readonly IServiceScopeFactory _scopeFactory;

        public NotificationListener(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        // Método para que el CommandHandler publique mensajes
        public void PublishMessage(NotificationEvent message)
        {
            _messageQueue.Enqueue(message);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_messageQueue.TryDequeue(out var notificationEvent))
                {
                    await ProcessNotificationAsync(notificationEvent);
                }
                await Task.Delay(100, stoppingToken);
            }
        }

        private async Task ProcessNotificationAsync(NotificationEvent evt)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<NotificationsDbContext>();

                var newLog = new NotificationLog
                {
                    IdAccountUser = evt.AccountId,
                    RecipientContact = evt.RecipientContact,
                    Channel = evt.Channel,
                    Subject = evt.Subject,
                    BodyContent = evt.Body,
                    CreatedDate = DateTime.Now
                };

                var attempt = new NotificationAttempt
                {
                    IdNotificationNavigation = newLog,
                    Status = "Sent",
                    ErrorMessage = "Procesado por Event-Bus Simulado",
                    AttemptDate = DateTime.Now
                };

                context.NotificationLogs.Add(newLog);
                context.NotificationAttempts.Add(attempt);
                await context.SaveChangesAsync();
            }
        }
    }
}