using MediatR;
using Microsoft.AspNetCore.Mvc;
using Neighborhood.Services.Application.Notifications.Push_inApp.Commands;
using Neighborhood.Services.Application.Notifications.Push_inApp.DTOs;
using Neighborhood.Services.Application.Notifications.Push_inApp.Queries;
using Neighborhood.Services.Application.Notifications.Services;
using Neighborhood.Services.Infrastructure.Services.NotificationService;

namespace Neighborhood.Services.API.Controllers.Notification
{

    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private IMediator _mediator;
        private INotificationService _service;
        public NotificationsController(IMediator mediator, INotificationService service)
        {
            _mediator = mediator;
            _service = service;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<PushNotificationDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllNotfsQDto());
            return Ok(result);
        }

        [HttpPost("SendingToAll")]
        public async Task<ActionResult> CreateNotification(string mssg)
        {
            var result = await _service.SendNotificationAsync(mssg);
            return Ok(result);
        }

        [HttpPut("MarkAllAsRead")]
        public async Task<IActionResult> MarkAllAsRead(MarkAllAsReadCommandDto command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("MarkAsRead/{id:int}")]
        public async Task<IActionResult> MarkAsRead(int id, MarkAsReadCommandDto command)
        {
            command.NotificationId = id;
           
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
