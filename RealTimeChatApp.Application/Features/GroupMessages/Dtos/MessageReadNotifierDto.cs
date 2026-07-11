using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.GroupMessages.Dtos
{
    public class MessageReadNotifierDto
    {
        public int GroupId { get; set; }

        public int MessageId { get; set; }

        public string UserId { get; set; } = null!;

        public DateTime ReadAt { get; set; }
    }
}
