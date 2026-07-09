using RealTimeChatApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Domain.Models
{
    public class Group : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public string CreatedById { get; set; } = null!;

        public bool IsPrivate { get; set; }

        public int MaxMembers { get; set; }

        public string? InviteCode { get; set; }
        public bool OnlyAdminsCanSendMessages { get; set; }
        // Navigation

        public ApplicationUser CreatedBy { get; set; } = null!;

        public ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();

        public ICollection<GroupMessage> Messages { get; set; } = new List<GroupMessage>();
    }
}
