using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Commons.Caching
{
    public static class CacheKeys
    {
        public static class Stories
        {
            public static string Feed(string userId)
                => $"stories:feed:{userId}";

            public static string Viewers(int storyId)
                => $"stories:viewers:{storyId}";

            public static string Reactions(int storyId,string userId)
                => $"stories:reactions:{storyId} {userId}";
        }

        public static class Groups
        {
            public static string Details(int groupId)
                => $"groups:{groupId}";

            public static string Members(int groupId)
                => $"groups:{groupId}:members";

            public static string MyGroups(string userId)
                => $"users:{userId}:groups";
        }

        public static class Chats
        {
            public static string Conversation(
                string userId,
                string otherUserId)
                => $"conversation:{userId}:{otherUserId}";

            public static string UserChats(string userId)
                => $"users:{userId}:chats";
        }

        public static class Messages
        {
            public static string Readers(int messageId)
                => $"messages:{messageId}:readers";
        }
    }
}
