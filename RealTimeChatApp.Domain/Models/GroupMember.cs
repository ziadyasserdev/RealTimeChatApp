using RealTimeChatApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Domain.Models
{
    public class GroupMember : BaseEntity
    {
        public int GroupId { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; } = null!;

        public string Role { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.Now;

        public string? Nickname { get; set; }

        public bool IsMuted { get; set; }

        public DateTime? MutedUntil { get; set; }

        public Guid? LastReadMessageId { get; set; }

        // Navigation

        public Group Group { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;
    }
}
