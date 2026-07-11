using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RealTimeChatApp.Api.Commons.Responses;
using RealTimeChatApp.Api.Hubs;
using RealTimeChatApp.Api.SignalR.NotifierService;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.GroupMessages.Commands.DeleteMessage;
using RealTimeChatApp.Application.Features.GroupMessages.Commands.EditGroupMessage;
using RealTimeChatApp.Application.Features.GroupMessages.Commands.MarkAsRead;
using RealTimeChatApp.Application.Features.GroupMessages.Commands.PinMessage;
using RealTimeChatApp.Application.Features.GroupMessages.Commands.SendTextMessage;
using RealTimeChatApp.Application.Features.GroupMessages.Commands.UnPinMessage;
using RealTimeChatApp.Application.Features.GroupMessages.Queries.GetMessageReaders;
using Swashbuckle.AspNetCore.Annotations;

namespace RealTimeChatApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupMessagesController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IChatHubNotifier chatHubNotifier;
       
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GroupMessagesController(IMediator mediator
            , IChatHubNotifier chatHubNotifier
            , IUnitOfWork unitOfWork
            , ICurrentUserService currentUserService)
        {
            this.mediator = mediator;
            this.chatHubNotifier = chatHubNotifier;
          
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        [HttpPost("{groupId}/messages/text")]
        [SwaggerOperation(
    Summary = "Send text message",
    Description = "Sends a text message to a chat group."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SendTextMessage(
    int groupId,
    [FromBody] SendTextMessageCommand command)
        {
            var result = await mediator.Send(command);

            if (!result.IsSuccess)
                return result.ToActionResult();

            await chatHubNotifier.SendGroupMessageAsync(result.Value!);

            return result.ToActionResult();
        }
        [HttpPut("messages/{messageId}")]
        [SwaggerOperation(
    Summary = "Edit group message",
    Description = "Allows the sender to edit his own message."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditMessage(
    int messageId,
    [FromBody] EditGroupMessageCommand command)
        {
            var result = await mediator.Send(command with
            {
                MessageId = messageId
            });

            if (result.IsSuccess)
            {
                await chatHubNotifier.MessageEditedAsync(result.Value!);
            }

            return result.ToActionResult();
        }
        [HttpDelete("messages/{messageId}")]
        [SwaggerOperation(
    Summary = "Delete group message",
    Description = "Deletes a group message."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            var result = await mediator.Send(
                new DeleteGroupMessageCommand(messageId));

            if (result.IsSuccess)
            {
                await chatHubNotifier.MessageDeletedAsync(result.Value!);
            }

            return result.ToActionResult();
        }
        [HttpPut("messages/{messageId}/pin")]
        [SwaggerOperation(
    Summary = "Pin group message",
    Description = "Pins a message in the group. Only Owner or Admin can pin messages."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PinMessage(int messageId)
        {
            var result = await mediator.Send(
                new PinGroupMessageCommand(messageId));

            if (result.IsSuccess)
            {
                await chatHubNotifier.MessagePinnedAsync(result.Value!);
            }

            return result.ToActionResult();
        }
        [HttpPut("messages/{messageId}/unpin")]
        [SwaggerOperation(
    Summary = "Unpin group message",
    Description = "Removes a pinned message from the group. Only Owner or Admin can unpin messages."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UnPinMessage(int messageId)
        {
            var result = await mediator.Send(
                new UnPinGroupMessageCommand(messageId));

            if (result.IsSuccess)
            {
                await chatHubNotifier.MessageUnPinnedAsync(result.Value!);
            }

            return result.ToActionResult();
        }
        [HttpPost("messages/{messageId}/read")]
        [SwaggerOperation(
    Summary = "Mark message as read",
    Description = "Marks a group message as read by the current user."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkAsRead(int messageId)
        {
            var result = await mediator.Send(
                new MarkGroupMessageAsReadCommand(messageId));

            if (result.IsSuccess)
            {
                await chatHubNotifier.MessageReadAsync(result.Value!);
            }

            return result.ToActionResult();
        }
        [HttpGet("messages/{messageId}/readers")]
        [SwaggerOperation(
    Summary = "Get message readers",
    Description = "Returns all users who have read the specified group message."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMessageReaders(int messageId)
        {
            var result = await mediator.Send(
                new GetMessageReadersQuery(messageId));

            return result.ToActionResult();
        }
    }
}
