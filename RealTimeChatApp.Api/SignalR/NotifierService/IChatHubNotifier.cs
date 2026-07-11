using RealTimeChatApp.Application.Features.GroupMessages.Dtos;
using RealTimeChatApp.Application.Features.Groups.Dtos;

namespace RealTimeChatApp.Api.SignalR.NotifierService
{
    public interface IChatHubNotifier
    {
        Task SendGroupMessageAsync(GroupMessageNotifierDto message);
        Task MessagePinnedAsync(MessagePinnedNotifierDto dto);
        Task MessageUnPinnedAsync(MessageUnPinnedNotifierDto dto);
        Task MessageReadAsync(MessageReadNotifierDto dto);
        Task UserJoinedGroupAsync(int groupId, string userName);
        Task MessageDeletedAsync(MessageDeletedNotifierDto dto);
        Task UserLeftGroupAsync(int groupId, string userName);
        Task MessageEditedAsync(EditGroupMessageNotifierDto message);
    }

   
}
