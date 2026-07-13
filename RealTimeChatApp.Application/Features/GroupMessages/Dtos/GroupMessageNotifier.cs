using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.GroupMessages.Dtos
{
    public class GroupMessageNotifierDto
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public string SenderId { get; set; } = null!;

        public string SenderName { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string MessageType { get; set; } = null!;

        public DateTime SentAt { get; set; }

        public bool IsEdited { get; set; }

        public DateTime? EditedAt { get; set; }

        public bool IsPinned { get; set; }
        public int? ReplyToMessageId { get; set; }

        public string? ReplyContent { get; set; }

        public string? ReplySenderName { get; set; }
        public bool IsForwarded { get; set; }

        public string? ForwardedFromUserId { get; set; }

        public string? ForwardedFromUserName { get; set; }
    }
}
