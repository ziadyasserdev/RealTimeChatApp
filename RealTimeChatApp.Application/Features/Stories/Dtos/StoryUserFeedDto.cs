using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Dtos
{
    public class StoryUserFeedDto
    {
        public string UserId { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string? ProfileImage { get; set; }

        public bool HasUnViewedStories { get; set; }

        public List<StoryDto> Stories { get; set; } = [];
    }
}
