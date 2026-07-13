using RealTimeChatApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.PrivateMessages.Dtos
{
    public class PrivateMessageNotifierDto
    {
        public int MessageId { get; set; }

        public string SenderId { get; set; } = null!;

        public string SenderName { get; set; } = null!;

        public string ReceiverId { get; set; } = null!;

        public string Content { get; set; } = null!;

        public MessageType MessageType { get; set; }

        public bool IsEdited { get; set; }

        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }

        public int? ReplyToMessageId { get; set; }
        public bool IsForwarded { get; set; }

        public string? ForwardedFromUserId { get; set; }

        public string? ForwardedFromUserName { get; set; }
    }
}
