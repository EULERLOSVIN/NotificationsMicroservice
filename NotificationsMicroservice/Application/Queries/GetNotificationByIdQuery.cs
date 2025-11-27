using MediatR;
using Microsoft.EntityFrameworkCore;
using NotificationsMicroservice.Data;
using NotificationsMicroservice.Entities;

namespace NotificationsMicroservice.Features.Queries
{
    // La Petición
    public class GetNotificationByIdQuery : IRequest<NotificationLog?>
    {
        public int Id { get; set; }
    }

    // El Handler de Lectura
    public class GetNotificationByIdHandler : IRequestHandler<GetNotificationByIdQuery, NotificationLog?>
    {
        private readonly NotificationsDbContext _context;

        public GetNotificationByIdHandler(NotificationsDbContext context)
        {
            _context = context;
        }

        public async Task<NotificationLog?> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
        {
            // Lógica pura de lectura (Query)
            return await _context.NotificationLogs
                .Include(n => n.NotificationAttempts) // Incluimos los intentos
                .FirstOrDefaultAsync(n => n.IdNotification == request.Id, cancellationToken);
        }
    }
}