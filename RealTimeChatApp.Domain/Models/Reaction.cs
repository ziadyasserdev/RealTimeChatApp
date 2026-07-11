using RealTimeChatApp.Domain.Enums;
using RealTimeChatApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Domain.Models
{
    public class Reaction : BaseEntity
    {
        public int? GroupMessageId { get; set; }

        public int? PrivateMessageId { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;

        public ReactionType ReactionType { get; set; }

        public DateTime ReactedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;

        // Navigation

        public GroupMessage? GroupMessage { get; set; }

        public PrivateMessage? PrivateMessage { get; set; }

        public ApplicationUser User { get; set; } = null!;
    }
}
