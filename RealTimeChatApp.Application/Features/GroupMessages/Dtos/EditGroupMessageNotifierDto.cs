using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.GroupMessages.Dtos
{
    public class EditGroupMessageNotifierDto
    {
        public int GroupId { get; set; }

        public int MessageId { get; set; }

        public string Content { get; set; } = null!;

        public bool IsEdited { get; set; }

        public DateTime? EditedAt { get; set; }
    }
}
