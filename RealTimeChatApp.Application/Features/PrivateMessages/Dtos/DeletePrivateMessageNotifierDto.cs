using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.PrivateMessages.Dtos
{
    public class DeletePrivateMessageNotifierDto
    {
        public int MessageId { get; set; }

        public string UserId { get; set; } = null!;
        
    }
}
