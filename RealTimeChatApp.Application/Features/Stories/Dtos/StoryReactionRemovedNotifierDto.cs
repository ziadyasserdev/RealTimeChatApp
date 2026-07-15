using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Dtos
{
    public sealed class StoryReactionRemovedNotifierDto
    {
        public int StoryId { get; set; }

        public string UserId { get; set; } = null!;
    }
}
