using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Dtos
{

    public class StoryDeletedNotifierDto
    {
        public int StoryId { get; set; }

        public string UserId { get; set; } = null!;
    }
}
