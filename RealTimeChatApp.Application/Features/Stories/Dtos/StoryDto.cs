using RealTimeChatApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Dtos
{
    public class StoryDto
    {
        public int StoryId { get; set; }

        public StoryType StoryType { get; set; }

        public string? MediaUrl { get; set; }

        public string? Caption { get; set; }

        public string? Text { get; set; }

        public string? BackgroundColor { get; set; }

        public string? TextColor { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsViewed { get; set; }
    }
}
