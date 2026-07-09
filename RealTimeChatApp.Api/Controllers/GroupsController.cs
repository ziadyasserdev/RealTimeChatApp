using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Extensibility;
using RealTimeChatApp.Api.Commons.Responses;
using RealTimeChatApp.Application.Features.Groups.Commands;
using RealTimeChatApp.Application.Features.Groups.Commands.CreateGroup;
using RealTimeChatApp.Application.Features.Groups.Commands.JoinGroup;
using Swashbuckle.AspNetCore.Annotations;

namespace RealTimeChatApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IMediator mediator;

        public GroupsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        [SwaggerOperation(
           Summary = "Create a new group",
           Description = "Creates a new chat group and assigns the current authenticated user as the group owner."
       )]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateGroup([FromQuery] CreateGroupCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpPost("{groupId}/join")]
        [SwaggerOperation(
    Summary = "Join a group",
    Description = "Allows the authenticated user to join a group using the group ID. If the group is private, an invite code is required."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> JoinGroup(
    int groupId,
    [FromBody] JoinGroupCommand command)
        {
            command.GroupId = groupId;

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

    }
}
