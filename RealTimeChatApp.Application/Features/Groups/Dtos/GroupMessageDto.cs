using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Dtos
{
    public class GroupMessageDto
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public string SenderId { get; set; } = default!;

        public string SenderName { get; set; } = default!;

        public string Content { get; set; } = default!;

        public string MessageType { get; set; } = default!;

        public DateTime SentAt { get; set; }
    }
}
