using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.Groups.Dtos;
using RealTimeChatApp.Domain.Identity;
using RealTimeChatApp.Domain.Models;

namespace RealTimeChatApp.Api.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ChatHub> _logger;
        private readonly UserManager<ApplicationUser> userManager;

        public ChatHub(
            IUnitOfWork unitOfWork,
            ILogger<ChatHub> logger,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            this.userManager = userManager;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;

            if (string.IsNullOrWhiteSpace(userId))
                throw new HubException("Unauthorized.");

            var user = await userManager.Users
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null)
                throw new HubException("User not found.");

            user.IsOnline = true;
            user.LastSeenAt = null;

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

           
            await Clients.All.SendAsync("UserOnline", new
            {
                UserId = user.Id,
                DisplayName = user.DisplayName
            });

            _logger.LogInformation(
                "User {UserId} connected with ConnectionId {ConnectionId}.",
                userId,
                Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public async Task StartTyping(string receiverId)
        {
            var senderId = Context.UserIdentifier;

            if (string.IsNullOrWhiteSpace(senderId))
                throw new HubException("Unauthorized.");

            var sender = await userManager.Users
               
                .FirstOrDefaultAsync(x => x.Id == senderId);

            if (sender is null)
                throw new HubException("User not found.");

            await Clients.User(receiverId)
                .SendAsync(
                    "UserTyping",
                    new
                    {
                        SenderId = sender.Id,
                        SenderName = sender.DisplayName
                    });
        }
        public async Task StopTyping(string receiverId)
        {
            var senderId = Context.UserIdentifier;

            if (string.IsNullOrWhiteSpace(senderId))
                throw new HubException("Unauthorized.");

            await Clients.User(receiverId)
                .SendAsync(
                    "UserStoppedTyping",
                    new
                    {
                        SenderId = senderId
                    });
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {

            var connection = await _unitOfWork.UserConnections.Query()
     .FirstOrDefaultAsync(x =>
         x.ConnectionId == Context.ConnectionId);

            if (connection != null)
            {
                connection.IsActive = false;
                connection.DisconnectedAt = DateTime.Now;

              
                var hasActiveConnections =
                    await _unitOfWork.UserConnections.Query()
                        .AnyAsync(x =>
                            x.UserId == connection.UserId &&
                            x.ConnectionId != connection.ConnectionId &&
                            x.IsActive);

                if (!hasActiveConnections)
                {
                    var user = await userManager.Users
                        .FirstOrDefaultAsync(x =>
                            x.Id == connection.UserId);

                    if (user != null)
                    {
                        user.IsOnline = false;
                        user.LastSeenAt = DateTime.UtcNow;

                        await Clients.All.SendAsync("UserOffline", new
                        {
                            UserId = user.Id,
                            LastSeenAt = user.LastSeenAt
                        });
                    }
                }

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

       

        
    }
}
