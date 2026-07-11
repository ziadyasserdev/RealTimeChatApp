using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensibility;
using RealTimeChatApp.Api.Commons.Responses;
using RealTimeChatApp.Api.Hubs;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.Groups.Commands;
using RealTimeChatApp.Application.Features.Groups.Commands.AddMember;
using RealTimeChatApp.Application.Features.Groups.Commands.ChangeAnnouncementMode;
using RealTimeChatApp.Application.Features.Groups.Commands.CreateGroup;
using RealTimeChatApp.Application.Features.Groups.Commands.DeleteGroup;
using RealTimeChatApp.Application.Features.Groups.Commands.DeleteGroupImage;
using RealTimeChatApp.Application.Features.Groups.Commands.DemoteMember;
using RealTimeChatApp.Application.Features.Groups.Commands.JoinGroup;
using RealTimeChatApp.Application.Features.Groups.Commands.LeaveGroup;
using RealTimeChatApp.Application.Features.Groups.Commands.MuteMember;
using RealTimeChatApp.Application.Features.Groups.Commands.PromoteMember;
using RealTimeChatApp.Application.Features.Groups.Commands.RemoveMember;
using RealTimeChatApp.Application.Features.Groups.Commands.UnMuteMember;
using RealTimeChatApp.Application.Features.Groups.Commands.UpdateGroup;
using RealTimeChatApp.Application.Features.Groups.Commands.UploadGroupImage;
using RealTimeChatApp.Application.Features.Groups.Queries.GetGroupDetails;
using RealTimeChatApp.Application.Features.Groups.Queries.GetGroupMembers;
//using RealTimeChatApp.Application.Features.Groups.Queries.GetGroupMessages;
using RealTimeChatApp.Application.Features.Groups.Queries.GetMyGroups;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace RealTimeChatApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IHubContext<ChatHub> hubContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GroupsController(IMediator mediator
            , IHubContext<ChatHub> hubContext
            , IUnitOfWork unitOfWork
            , ICurrentUserService currentUserService)
        {
            this.mediator = mediator;
            this.hubContext = hubContext;
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
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
        [HttpDelete("{groupId}/leave")]
        [SwaggerOperation(
       Summary = "Leave Group",
       Description = "Allows the current authenticated user to leave the group."
   )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> LeaveGroup(int groupId)
        {
            var command = new LeaveGroupCommand(groupId);

            var result = await mediator.Send(command);

            if (!result.IsSuccess)
                return result.ToActionResult();


            var currentUserId = currentUserService.UserId!;

            var connections = await unitOfWork.UserConnections
                .Query()
                .Where(x => x.UserId == currentUserId && x.IsActive)
                .ToListAsync();

            foreach (var connection in connections)
            {
                await hubContext.Groups.RemoveFromGroupAsync(
                    connection.ConnectionId,
                    $"group-{groupId}");
            }


            await hubContext.Clients
                .Group($"group-{groupId}")
                .SendAsync(
                    "UserLeftGroup",
                    new
                    {
                        GroupId = groupId,
                        UserId = currentUserId
                    });

            return result.ToActionResult();
        }
     

//        [HttpGet("{groupId}/messages")]
//        [SwaggerOperation(
//    Summary = "Get group messages",
//    Description = "Retrieves paginated messages for the specified group."
//)]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> GetGroupMessages(
//    int groupId,
//    [FromQuery] int page = 1,
//    [FromQuery] int pageSize = 20)
//        {
//            var query = new GetGroupMessagesQuery(groupId, page, pageSize);

//            var result = await mediator.Send(query);
//            return result.ToActionResult();
//        }

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
        [HttpDelete("{groupId}/members/{userId}")]
        [SwaggerOperation(
            Summary = "Remove a member from the group",
            Description = "Removes the specified member from the group. Only the group owner or admins can perform this action."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveMember(
            int groupId,
            string userId)
        {
            var command = new RemoveMemberCommand(groupId, userId);

            var result = await mediator.Send(command);

            if (!result.IsSuccess)
                return result.ToActionResult();


            var connections = await unitOfWork.UserConnections
                .Query()
                .Where(x => x.UserId == userId && x.IsActive)
                .ToListAsync();


            foreach (var connection in connections)
            {
                await hubContext.Groups.RemoveFromGroupAsync(
                    connection.ConnectionId,
                    $"group-{groupId}");
            }


            await hubContext.Clients
                .Group($"group-{groupId}")
                .SendAsync(
                    "MemberRemoved",
                    new
                    {
                        GroupId = groupId,
                        UserId = userId
                    });

            return result.ToActionResult();
        }
        [HttpPut("{groupId}")]
        [SwaggerOperation(
    Summary = "Update Group",
    Description = "Update group information."
)]
        public async Task<IActionResult> UpdateGroup(
    int groupId,
    UpdateGroupCommand command)
        {
            if (groupId != command.GroupId)
                return BadRequest();

            var result = await mediator.Send(command);

            if (!result.IsSuccess)
                return result.ToActionResult();

            await hubContext.Clients
                .Group($"group-{groupId}")
                .SendAsync("GroupUpdated");

            return result.ToActionResult();
        }


        [HttpPut("{groupId}/members/{userId}/promote")]
        public async Task<IActionResult> Promote(
    int groupId,
    string userId)
        {
            var result = await mediator.Send(
                new PromoteMemberCommand(groupId, userId));

            if (!result.IsSuccess)
                return result.ToActionResult();

            await hubContext.Clients
                .Group($"group-{groupId}")
                .SendAsync(
                    "MemberPromoted",
                    userId);

            return result.ToActionResult();
        }
        [HttpPut("{groupId}/members/{userId}/demote")]
        public async Task<IActionResult> Demote(
    int groupId,
    string userId)
        {
            var result = await mediator.Send(
                new DemoteMemberCommand(groupId, userId));

            if (!result.IsSuccess)
                return result.ToActionResult();

            await hubContext.Clients
                .Group($"group-{groupId}")
                .SendAsync(
                    "MemberDemoted",
                    userId);

            return result.ToActionResult();
        }
        [HttpPost("{groupId}/image")]
        public async Task<IActionResult> UploadGroupImage(
    int groupId,
     IFormFile image)
        {
            var result = await mediator.Send(
                new UploadGroupImageCommand(groupId, image));

            if (!result.IsSuccess)
                return result.ToActionResult();

            await hubContext.Clients
                .Group($"group-{groupId}")
                .SendAsync("GroupImageUpdated", result.Value);

            return result.ToActionResult();
        }
        [HttpPost("{groupId}/members")]
        [SwaggerOperation(
    Summary = "Add member to group",
    Description = "Adds a user to the group. Only Owner or Admin can add members."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddMember(
    int groupId,
    AddMemberCommand command)
        {
            if (groupId != command.GroupId)
                return BadRequest();

            var result = await mediator.Send(command);

            if (!result.IsSuccess)
                return result.ToActionResult();

           
            var connections = await unitOfWork.UserConnections
                .Query()
                .Where(x =>
                    x.UserId == command.UserId &&
                    x.IsActive)
                .ToListAsync();

            foreach (var connection in connections)
            {
                await hubContext.Groups.AddToGroupAsync(
                    connection.ConnectionId,
                    $"group-{groupId}");
            }

           
            await hubContext.Clients
                .Group($"group-{groupId}")
                .SendAsync(
                    "MemberAdded",
                    command.UserId,
                    groupId);

            return result.ToActionResult();
        }

        [HttpDelete("{groupId}/image")]
        [SwaggerOperation(
    Summary = "Delete group image",
    Description = "Deletes the image of the specified group."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteGroupImage(int groupId)
        {
            var result = await mediator.Send(new DeleteGroupImageCommand(groupId));
            
            return result.ToActionResult();
        }

        [HttpPost("{groupId}/members/{userId}/mute")]
        public async Task<IActionResult> MuteMember(
    int groupId,
    string userId,
    [FromBody] MuteMemberCommand request)
        {
            var result = await mediator.Send(
                new MuteMemberCommand(
                    groupId,
                    userId,
                    request.MutedUntil));

            if (!result.IsSuccess)
                return result.ToActionResult();

            await hubContext.Clients
                .Group($"group-{groupId}")
                .SendAsync(
                    "MemberMuted",
                    userId,
                    request.MutedUntil);

            return result.ToActionResult();
        }
        [HttpPut("{groupId}/members/{userId}/unmute")]
        public async Task<IActionResult> UnMuteMember(
    int groupId,
    string userId)
        {
            var result = await mediator.Send(
                new UnMuteMemberCommand(groupId, userId));

            if (!result.IsSuccess)
                return result.ToActionResult();

            await hubContext.Clients
                .Group($"group-{groupId}")
                .SendAsync(
                    "MemberUnMuted",
                    userId);

            return result.ToActionResult();
        }

        [HttpPut("{groupId}/announcement-mode")]
        [SwaggerOperation(
    Summary = "Change announcement mode",
    Description = "Enables or disables announcement mode. When enabled, only the group owner and admins can send messages."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeAnnouncementMode(
    int groupId,
    [FromBody] ChangeAnnouncementModeCommand request)
        {
            var command = new ChangeAnnouncementModeCommand(
                groupId,
                request.Enable);

            var result = await mediator.Send(command);
           
            return result.ToActionResult();
        }
    }
}
