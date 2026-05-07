using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Features.Notifications.Commands.MarkAllNotificationsAsRead;
using GraduationProject.Application.Features.Notifications.Commands.MarkNotificationAsRead;
using GraduationProject.Application.Features.Notifications.Queries.GetNotifications;
using GraduationProject.Application.Features.Notifications.Queries.GetUnreadNotificationsCount;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator mediator;

        public NotificationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [SwaggerOperation(
    Summary = "Get user notifications",
    Description = "Retrieves all notifications for the currently authenticated user."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetNotifications()
        {
            var result = await mediator.Send(new GetNotificationsQuery());

            return result.ToActionResult();
        }

        [HttpPatch("{notificationId}/mark-as-read")]
        [SwaggerOperation(
    Summary = "Mark notification as read",
    Description = "Marks the specified notification as read for the authenticated user."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkAsRead(
    [SwaggerParameter(Description = "The unique identifier of the notification.")]
    int notificationId)
        {
            var result = await mediator.Send(new MarkNotificationAsReadCommand
            {
                NotificationId = notificationId
            });

            return result.ToActionResult();
        }

        [HttpPatch("mark-all-as-read")]
        [SwaggerOperation(
    Summary = "Mark all notifications as read",
    Description = "Marks all notifications for the authenticated user as read."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var result = await mediator.Send(new MarkAllNotificationsAsReadCommand());

            return result.ToActionResult();
        }

        [HttpGet("unread-count")]
        [SwaggerOperation(
    Summary = "Get unread notifications count",
    Description = "Retrieves the total number of unread notifications for the authenticated user."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUnreadCount()
        {
            var result = await mediator.Send(new GetUnreadNotificationsCountQuery());

            return result.ToActionResult();
        }
    }
}
