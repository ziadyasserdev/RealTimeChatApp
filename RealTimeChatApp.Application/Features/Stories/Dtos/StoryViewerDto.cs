using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Dtos
{
    public class StoryViewerDto
    {
        public string ViewerId { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string? ProfilePictureUrl { get; set; }

        public DateTime ViewedAt { get; set; }
    }
}
