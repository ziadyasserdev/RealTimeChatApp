using RealTimeChatApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Dtos
{
    public sealed class StoryReactionNotifierDto
    {
        public int StoryId { get; set; }

        public string UserId { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string? ProfilePictureUrl { get; set; }

        public ReactionType Type { get; set; }

        public DateTime ReactedAt { get; set; }

        public bool IsNewReaction { get; set; }
    }
}
