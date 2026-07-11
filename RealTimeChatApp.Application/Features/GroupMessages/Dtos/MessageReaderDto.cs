using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.GroupMessages.Dtos
{
    public class MessageReaderDto
    {
        public string UserId { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public DateTime ReadAt { get; set; }
    }

}
