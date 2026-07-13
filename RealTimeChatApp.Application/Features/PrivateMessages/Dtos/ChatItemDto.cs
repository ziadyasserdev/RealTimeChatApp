using RealTimeChatApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.PrivateMessages.Dtos
{
    public class ChatItemDto
    {
        public string UserId { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string? LastMessage { get; set; }

        public DateTime? LastMessageDate { get; set; }
        public DateTime? LastSeenAt { get; set; }
        public MessageType? LastMessageType { get; set; }

        public bool IsOnline { get; set; }

        public int UnreadCount { get; set; }
    }
}
