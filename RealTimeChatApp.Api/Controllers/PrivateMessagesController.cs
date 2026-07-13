using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeChatApp.Api.Commons.Responses;
using RealTimeChatApp.Api.SignalR.NotifierService;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.PrivateMessages.Commands.DeleteForMe;
using RealTimeChatApp.Application.Features.PrivateMessages.Commands.DeletePrivateMessageForEveryone;
using RealTimeChatApp.Application.Features.PrivateMessages.Commands.EditMessage;
using RealTimeChatApp.Application.Features.PrivateMessages.Commands.MarkAsRead;
using RealTimeChatApp.Application.Features.PrivateMessages.Commands.SendTextMessage;
using RealTimeChatApp.Application.Features.PrivateMessages.Queries.GetChats;
using RealTimeChatApp.Application.Features.PrivateMessages.Queries.GetConversation;
using RealTimeChatApp.Application.Features.Reactions.Commands.ReactToPrivateMessage;
using Swashbuckle.AspNetCore.Annotations;

namespace RealTimeChatApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrivateMessagesController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IChatHubNotifier chatHubNotifier;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public PrivateMessagesController(IMediator mediator
            , IChatHubNotifier chatHubNotifier
            , IUnitOfWork unitOfWork
            , ICurrentUserService currentUserService)
        {
            this.mediator = mediator;
            this.chatHubNotifier = chatHubNotifier;
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        [HttpPost("text")]
        [SwaggerOperation(
    Summary = "Send private text message",
    Description = "Sends a private text message to another user."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SendTextMessage(
    [FromBody] SendPrivateMessageCommand command)
        {
            var result = await mediator.Send(command);

            if (result.IsSuccess)
            {
                await chatHubNotifier.SendPrivateMessageAsync(result.Value!);
            }

            return result.ToActionResult();
        }

        [HttpGet("{userId}")]
        [SwaggerOperation(
    Summary = "Get conversation",
    Description = "Returns all messages between the current user and another user."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetConversation(
    string userId,
    [FromQuery] GetConversationQuery query)
        {
            var result = await mediator.Send(
                query with
                {
                    UserId = userId
                });

            return result.ToActionResult();
        }
        [HttpPut("{messageId}")]
        [SwaggerOperation(
    Summary = "Edit private message",
    Description = "Allows the sender to edit a private message."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditMessage(
    int messageId,
    [FromBody] EditPrivateMessageCommand command)
        {
            var result = await mediator.Send(
                command with
                {
                    MessageId = messageId
                });

            if (result.IsSuccess)
            {
                await chatHubNotifier.PrivateMessageEditedAsync(result.Value!);
            }

            return result.ToActionResult();
        }
        [HttpDelete("{messageId}/me")]
        [SwaggerOperation(
    Summary = "Delete private message for me",
    Description = "Deletes the private message only for the current authenticated user."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteForMe(int messageId)
        {
            var result = await mediator.Send(
                new DeletePrivateMessageForMeCommand(messageId));

            if (result.IsSuccess)
            {
                await chatHubNotifier.PrivateMessageDeletedForMeAsync(result.Value!);
            }

            return result.ToActionResult();
        }
        [HttpDelete("{messageId}/everyone")]
        [SwaggerOperation(
    Summary = "Delete private message for everyone",
    Description = "Deletes the message for both sender and receiver."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteForEveryone(int messageId)
        {
            var result = await mediator.Send(
                new DeletePrivateMessageForEveryoneCommand(messageId));

            if (result.IsSuccess)
            {
                await chatHubNotifier.PrivateMessageDeletedForEveryoneAsync(result.Value!);
            }

            return result.ToActionResult();
        }
        [HttpPost("{messageId}/read")]
        [SwaggerOperation(
    Summary = "Mark private message as read",
    Description = "Marks a private message as read by the receiver."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkAsRead(int messageId)
        {
            var result = await mediator.Send(
                new MarkPrivateMessageAsReadCommand(messageId));

            if (result.IsSuccess)
            {
                await chatHubNotifier.PrivateMessageReadAsync(result.Value!);
            }

            return result.ToActionResult();
        }
        [HttpGet("chats")]
        [SwaggerOperation(
    Summary = "Get chats",
    Description = "Returns all chats for the authenticated user with last message and unread count."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetChats()
        {
            var result = await mediator.Send(new GetChatsQuery());

            return result.ToActionResult();
        }
        [HttpPost("{messageId}/reaction")]
        [SwaggerOperation(
    Summary = "React to private message",
    Description = "Adds or updates a reaction on a private message."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReactToMessage(
    int messageId,
    [FromBody] ReactToPrivateMessageCommand command)
        {
            var result = await mediator.Send(
                command with
                {
                    MessageId = messageId
                });

            if (result.IsSuccess)
            {
                await chatHubNotifier
                    .PrivateReactionChangedAsync(result.Value!);
            }

            return result.ToActionResult();
        }
    }
}
