using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Extensibility;
using RealTimeChatApp.Api.Commons.Responses;
using RealTimeChatApp.Application.Features.Groups.Commands;
using RealTimeChatApp.Application.Features.Groups.Commands.CreateGroup;
using RealTimeChatApp.Application.Features.Groups.Commands.JoinGroup;
using RealTimeChatApp.Application.Features.Groups.Commands.LeaveGroup;
using RealTimeChatApp.Application.Features.Groups.Commands.SendGroupMessage;
using RealTimeChatApp.Application.Features.Groups.Queries.GetGroupDetails;
using RealTimeChatApp.Application.Features.Groups.Queries.GetGroupMembers;
using RealTimeChatApp.Application.Features.Groups.Queries.GetGroupMessages;
using RealTimeChatApp.Application.Features.Groups.Queries.GetMyGroups;
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
        [HttpPost("{groupId}/leave")]
        [SwaggerOperation(
    Summary = "Leave a group",
    Description = "Allows the authenticated user to leave the specified group."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> LeaveGroup(int groupId)
        {
            var command = new LeaveGroupCommand(groupId);

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpPost("{groupId}/messages")]
        [SwaggerOperation(
    Summary = "Send a message to a group",
    Description = "Sends a new message to the specified group."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SendMessage(
    int groupId,
    [FromBody] SendGroupMessageCommand request)
        {
            var command = new SendGroupMessageCommand(groupId, request.Content);

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpGet("{groupId}/messages")]
        [SwaggerOperation(
    Summary = "Get group messages",
    Description = "Retrieves paginated messages for the specified group."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetGroupMessages(
    int groupId,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20)
        {
            var query = new GetGroupMessagesQuery(groupId, page, pageSize);

            var result = await mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("my-groups")]
        [SwaggerOperation(
    Summary = "Get my groups",
    Description = "Retrieves all groups that the authenticated user is a member of."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyGroups()
        {
            var result = await mediator.Send(new GetMyGroupsQuery());
            return result.ToActionResult();
        }
        [HttpGet("{groupId}")]
        [SwaggerOperation(
    Summary = "Get group details",
    Description = "Retrieves the details of the specified group."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetGroupDetails(int groupId)
        {
            var result = await mediator.Send(new GetGroupDetailsQuery(groupId));

            return result.ToActionResult();
        }

        [HttpGet("{groupId}/members")]
        [SwaggerOperation(
    Summary = "Get group members",
    Description = "Retrieves all members of the specified group."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetGroupMembers(int groupId)
        {
            var result = await mediator.Send(new GetGroupMembersQuery(groupId));

            return result.ToActionResult();
        }
    }
}
