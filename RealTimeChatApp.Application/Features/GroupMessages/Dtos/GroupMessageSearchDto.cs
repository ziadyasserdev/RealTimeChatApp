using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.GroupMessages.Dtos
{
    public class GroupMessageSearchDto
    {
        public int MessageId { get; set; }

        public int GroupId { get; set; }

        public string SenderId { get; set; } = null!;

        public string SenderName { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime SentAt { get; set; }
    }
}
