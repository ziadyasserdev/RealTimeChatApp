using Microsoft.AspNetCore.SignalR;
using RealTimeChatApp.Api.Hubs;
using RealTimeChatApp.Application.Features.GroupMessages.Dtos;
using RealTimeChatApp.Application.Features.Groups.Dtos;
using RealTimeChatApp.Application.Features.PrivateMessages.Dtos;
using RealTimeChatApp.Application.Features.Reactions.Dtos;

namespace RealTimeChatApp.Api.SignalR.NotifierService
{

    public class ChatHubNotifier : IChatHubNotifier
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatHubNotifier(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task ReactionChangedAsync(
    ReactionNotifierDto dto)
        {
            await _hubContext.Clients
                .Group($"group-{dto.GroupId}")
                .SendAsync("ReactionChanged", dto);
        }
        public async Task MessageReadAsync(
    MessageReadNotifierDto dto)
        {
            await _hubContext.Clients
                .Group($"group-{dto.GroupId}")
                .SendAsync("MessageRead", dto);
        }
        public async Task MessageUnPinnedAsync(
    MessageUnPinnedNotifierDto dto)
        {
            await _hubContext.Clients
                .Group($"group-{dto.GroupId}")
                .SendAsync("MessageUnPinned", dto);
        }
        public async Task MessagePinnedAsync(
    MessagePinnedNotifierDto dto)
        {
            await _hubContext.Clients
                .Group($"group-{dto.GroupId}")
                .SendAsync("MessagePinned", dto);
        }
        public async Task MessageDeletedAsync(
    MessageDeletedNotifierDto dto)
        {
            await _hubContext.Clients
                .Group($"group-{dto.GroupId}")
                .SendAsync("MessageDeleted", dto);
        }
        public async Task MessageEditedAsync(
     EditGroupMessageNotifierDto message)
        {
            await _hubContext.Clients
                .Group($"group-{message.GroupId}")
                .SendAsync("MessageEdited", message);
        }
        public async Task SendPrivateMessageAsync(
    PrivateMessageNotifierDto dto)
        {
          
            await _hubContext.Clients
                .User(dto.ReceiverId)
                .SendAsync("ReceivePrivateMessage", dto);

          
            await _hubContext.Clients
                .User(dto.SenderId)
                .SendAsync("ReceiveMyPrivateMessage", dto);
        }
        public async Task PrivateMessageEditedAsync(
    PrivateMessageNotifierDto dto)
        {
            await _hubContext.Clients
                .User(dto.SenderId)
                .SendAsync("PrivateMessageEdited", dto);

            await _hubContext.Clients
                .User(dto.ReceiverId)
                .SendAsync("PrivateMessageEdited", dto);
        }
        public async Task PrivateMessageDeletedForMeAsync(
    DeletePrivateMessageNotifierDto dto)
        {
            await _hubContext.Clients
                .User(dto.UserId)
                .SendAsync(
                    "PrivateMessageDeletedForMe",
                    dto);
        }
        public async Task PrivateMessageDeletedForEveryoneAsync(
    PrivateMessageNotifierDto dto)
        {
            await _hubContext.Clients
                .User(dto.SenderId)
                .SendAsync("PrivateMessageDeletedForEveryone", dto);

            await _hubContext.Clients
                .User(dto.ReceiverId)
                .SendAsync("PrivateMessageDeletedForEveryone", dto);
        }
  
        public async Task PrivateMessageReadAsync(
    PrivateMessageNotifierDto dto)
        {
            await _hubContext.Clients
                .User(dto.SenderId)
                .SendAsync("PrivateMessageRead", dto);
        }
        public async Task SendGroupMessageAsync(GroupMessageNotifierDto message)
        {
            await _hubContext.Clients
                .Group($"group-{message.GroupId}")
                .SendAsync("ReceiveGroupMessage", message);
        }
        public async Task PrivateReactionChangedAsync(
    ReactionNotifierDto dto)
        {
            await _hubContext.Clients
                .Users(dto.SenderId, dto.ReceiverId)
                .SendAsync("PrivateReactionChanged", dto);
        }

        public async Task UserJoinedGroupAsync(int groupId, string userName)
        {
            await _hubContext.Clients
                .Group($"group-{groupId}")
                .SendAsync("UserJoinedGroup", userName);
        }

        public async Task UserLeftGroupAsync(int groupId, string userName)
        {
            await _hubContext.Clients
                .Group($"group-{groupId}")
                .SendAsync("UserLeftGroup", userName);
        }
    }
}
