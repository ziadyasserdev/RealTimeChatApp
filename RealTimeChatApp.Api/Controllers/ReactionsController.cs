using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeChatApp.Api.Commons.Responses;
using RealTimeChatApp.Api.SignalR.NotifierService;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.Reactions.Commands.ReactToGroupMessage;
using RealTimeChatApp.Application.Features.Reactions.Commands.ReactToPrivateMessage;
using Swashbuckle.AspNetCore.Annotations;

namespace RealTimeChatApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IChatHubNotifier chatHubNotifier;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public ReactionsController(IMediator mediator
            , IChatHubNotifier chatHubNotifier
            , IUnitOfWork unitOfWork
            , ICurrentUserService currentUserService)
        {
            this.mediator = mediator;
            this.chatHubNotifier = chatHubNotifier;
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        [HttpPost("group-messages/{messageId}")]
        [SwaggerOperation(
           Summary = "React to group message",
           Description = "Adds, changes or removes a reaction from a group message."
       )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReactToGroupMessage(
           int messageId,
           [FromQuery] ReactToGroupMessageCommand command)
        {
            var result = await mediator.Send(
                command with
                {
                    MessageId = messageId
                });

            if (result.IsSuccess)
            {
                await chatHubNotifier.ReactionChangedAsync(result.Value!);
            }

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
