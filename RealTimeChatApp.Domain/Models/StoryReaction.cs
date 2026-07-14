using RealTimeChatApp.Domain.Enums;
using RealTimeChatApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Domain.Models
{
    public class StoryReaction : BaseEntity
    {
        public int StoryId { get; set; }

        public string UserId { get; set; } = null!;

        public ReactionType ReactionType { get; set; }

        public DateTime ReactedAt { get; set; }

        public Story Story { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;
    }
}
