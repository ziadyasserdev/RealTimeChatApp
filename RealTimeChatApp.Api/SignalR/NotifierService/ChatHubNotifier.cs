using Microsoft.AspNetCore.SignalR;
using RealTimeChatApp.Api.Hubs;
using RealTimeChatApp.Application.Features.GroupMessages.Dtos;
using RealTimeChatApp.Application.Features.Groups.Dtos;

namespace RealTimeChatApp.Api.SignalR.NotifierService
{

    public class ChatHubNotifier : IChatHubNotifier
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatHubNotifier(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
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
        public async Task SendGroupMessageAsync(GroupMessageNotifierDto message)
        {
            await _hubContext.Clients
                .Group($"group-{message.GroupId}")
                .SendAsync("ReceiveGroupMessage", message);
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
