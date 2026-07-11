using RealTimeChatApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.PrivateMessages.Dtos
{
    public class PrivateMessageDto
    {
        public int Id { get; set; }

        public string SenderId { get; set; } = null!;

        public string SenderName { get; set; } = null!;

        public string ReceiverId { get; set; } = null!;

        public string Content { get; set; } = null!;

        public MessageType MessageType { get; set; }

        public bool IsEdited { get; set; }

        public DateTime SentAt { get; set; }

        public int? ReplyToMessageId { get; set; }

        public bool DeletedForMe { get; set; }
    }
}
