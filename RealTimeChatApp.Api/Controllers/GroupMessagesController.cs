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
using RealTimeChatApp.Application.Features.GroupMessages.Commands.SendTextMessage;
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
    }
}
