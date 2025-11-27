using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotificationsMicroservice.Features.Commands;
using NotificationsMicroservice.Features.Queries;

namespace NotificationsMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        // Solo inyectamos el Mediador. ¡Adiós dependencias complejas!
        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // --- WRITE SIDE (COMMAND) ---
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] CreateNotificationCommand command)
        {
            // El controlador delega todo al mediador
            var result = await _mediator.Send(command);
            return Accepted(new { Message = result });
        }

        // --- READ SIDE (QUERY) ---
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStatus(int id)
        {
            var query = new GetNotificationByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null) return NotFound();

            return Ok(result);
        }
    }
}