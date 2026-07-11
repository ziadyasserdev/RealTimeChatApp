using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.Groups.Dtos;
using RealTimeChatApp.Domain.Models;

namespace RealTimeChatApp.Api.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(
            IUnitOfWork unitOfWork,
            ILogger<ChatHub> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
       
            if (string.IsNullOrWhiteSpace(userId))
                throw new HubException("Unauthorized.");

            var now = DateTime.UtcNow;

            var connection = new UserConnection
            {
                UserId = userId,
                ConnectionId = Context.ConnectionId,
                ConnectedAt = now,
                CreatedAt = now,
                IsActive = true
            };

            await _unitOfWork.UserConnections.AddAsync(connection);

           
            var groupIds = await _unitOfWork.GroupMembers.Query()
                .Where(x => x.UserId == userId)
                .Select(x => x.GroupId)
                .ToListAsync();

            foreach (var groupId in groupIds)
            {
                await Groups.AddToGroupAsync(
                    Context.ConnectionId,
                    $"group-{groupId}");
            }

            await _unitOfWork.SaveAsync();

            _logger.LogInformation(
                "User {UserId} connected with ConnectionId {ConnectionId} and joined {GroupsCount} groups.",
                userId,
                Context.ConnectionId,
                groupIds.Count);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connection = await _unitOfWork.UserConnections.Query()
                .FirstOrDefaultAsync(x =>
                    x.ConnectionId == Context.ConnectionId);

            if (connection != null)
            {
                connection.IsActive = false;
                connection.DisconnectedAt = DateTime.UtcNow;

                await _unitOfWork.SaveAsync();

                _logger.LogInformation(
                    "User {UserId} disconnected.",
                    connection.UserId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinChatGroupAsync(int groupId)
        {
            var userId = Context.UserIdentifier;

            if (string.IsNullOrWhiteSpace(userId))
                throw new HubException("Unauthorized.");

            var isMember = await _unitOfWork.GroupMembers.Query()
                .AnyAsync(x =>
                    x.GroupId == groupId &&
                    x.UserId == userId);

            if (!isMember)
                throw new HubException("You are not a member of this group.");

            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                $"group-{groupId}");

            var userName = Context.User?.Identity?.Name ?? "Unknown";

            await Clients.OthersInGroup($"group-{groupId}")
                .SendAsync(
                    "UserJoinedGroup",
                    userName,
                    groupId);

            _logger.LogInformation(
                "User {UserId} joined SignalR group {GroupId}",
                userId,
                groupId);
        }

       

        //public async Task SendGroupMessageAsync(int messageId)
        //{
        //    var message = await _unitOfWork.GroupMessages.Query()

        //        .Include(x => x.Sender)

        //        .FirstOrDefaultAsync(x => x.Id == messageId);

        //    if (message is null)
        //        throw new HubException("Message not found.");

        //    await Clients.Group($"group-{message.GroupId}")

        //        .SendAsync(
        //            "ReceiveGroupMessage",

        //            new GroupMessageDto
        //            {
        //                Id = message.Id,
        //                GroupId = message.GroupId,
        //                SenderId = message.SenderId,
        //                SenderName = message.Sender.UserName!,
        //                Content = message.Content,
        //                MessageType = message.MessageType,
        //                SentAt = message.SentAt
        //            });
        //}
    }
}
