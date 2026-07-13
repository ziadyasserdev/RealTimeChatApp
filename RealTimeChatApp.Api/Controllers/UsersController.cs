using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RealTimeChatApp.Api.Commons.Responses;
using RealTimeChatApp.Api.Hubs;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.Users.Commands.BlockUser;
using RealTimeChatApp.Application.Features.Users.Commands.UnBlockUser;
using Swashbuckle.AspNetCore.Annotations;

namespace RealTimeChatApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IHubContext<ChatHub> hubContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public UsersController(IMediator mediator
            , IHubContext<ChatHub> hubContext
            , IUnitOfWork unitOfWork
            , ICurrentUserService currentUserService)
        {
            this.mediator = mediator;
            this.hubContext = hubContext;
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        [HttpPost("{userId}/block")]
        [SwaggerOperation(
       Summary = "Block user",
       Description = "Blocks another user. Blocked users cannot send messages or interact with you."
   )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> BlockUser(string userId)
        {
            var result = await mediator.Send(
                new BlockUserCommand(userId));

            return result.ToActionResult();
        }
        [HttpDelete("{userId}/block")]
        [SwaggerOperation(
    Summary = "Unblock user",
    Description = "Removes a previously blocked user."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UnBlockUser(string userId)
        {
            var result = await mediator.Send(
                new UnBlockUserCommand(userId));

            return result.ToActionResult();
        }
    }
}
