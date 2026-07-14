using RealTimeChatApp.Domain.Enums;
using RealTimeChatApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Domain.Models
{
    public class Story : BaseEntity
    {
        public string UserId { get; set; } = null!;

        public string? MediaUrl { get; set; }
        public string? MediaPublicId { get; set; }

        public StoryType Type { get; set; }

        public string? Caption { get; set; }

        public string? Text { get; set; }

        public string? BackgroundColor { get; set; }

        public string? TextColor { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public ApplicationUser User { get; set; } = null!;

        public ICollection<StoryView> Views { get; set; } = new List<StoryView>();
        public ICollection<StoryReaction> Reactions { get; set; }
= new List<StoryReaction>();
            
    }
}
