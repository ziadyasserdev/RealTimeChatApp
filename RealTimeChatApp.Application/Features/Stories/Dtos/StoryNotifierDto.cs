using RealTimeChatApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Dtos
{
    public sealed class StoryNotifierDto
    {
        public int StoryId { get; set; }

        public string UserId { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public StoryType StoryType { get; set; }

        public string? MediaUrl { get; set; }

        public string? Caption { get; set; }

        public string? Text { get; set; }

        public string? BackgroundColor { get; set; }

        public string? TextColor { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}
