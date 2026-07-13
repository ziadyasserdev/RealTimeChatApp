using RealTimeChatApp.Application.Features.GroupMessages.Dtos;
using RealTimeChatApp.Application.Features.Groups.Dtos;
using RealTimeChatApp.Application.Features.PrivateMessages.Dtos;
using RealTimeChatApp.Application.Features.Reactions.Dtos;
using RealTimeChatApp.Application.Features.Stories.Dtos;

namespace RealTimeChatApp.Api.SignalR.NotifierService
{
    public interface IChatHubNotifier
    {
        Task SendGroupMessageAsync(GroupMessageNotifierDto message);
        Task MessagePinnedAsync(MessagePinnedNotifierDto dto);
        Task MessageUnPinnedAsync(MessageUnPinnedNotifierDto dto);
        Task SendPrivateMessageAsync(PrivateMessageNotifierDto dto);
        Task MessageReadAsync(MessageReadNotifierDto dto);
        Task PrivateMessageDeletedForMeAsync(
    DeletePrivateMessageNotifierDto dto);
        Task PrivateMessageDeletedForEveryoneAsync(
    PrivateMessageNotifierDto dto);
        Task PrivateMessageReadAsync(
    PrivateMessageNotifierDto dto);
        Task PrivateMessageEditedAsync(
    PrivateMessageNotifierDto dto);
        Task ReactionChangedAsync(ReactionNotifierDto dto);
        Task UserJoinedGroupAsync(int groupId, string userName);
        Task MessageDeletedAsync(MessageDeletedNotifierDto dto);
        Task UserLeftGroupAsync(int groupId, string userName);
        Task MessageEditedAsync(EditGroupMessageNotifierDto message);
        Task PrivateReactionChangedAsync(
    ReactionNotifierDto dto);
        Task StoryCreatedAsync(StoryNotifierDto dto);

        Task StoryDeletedAsync(int storyId);

        Task StoryViewedAsync(int storyId, string viewerId);
        Task StoryDeletedAsync(StoryDeletedNotifierDto dto);
    }
   
}
