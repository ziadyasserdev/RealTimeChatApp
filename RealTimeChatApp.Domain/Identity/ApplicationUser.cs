using Microsoft.AspNetCore.Identity;
using RealTimeChatApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RealTimeChatApp.Domain.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; } = null!;

        public string? ProfilePictureUrl { get; set; }

        public string? Bio { get; set; }

        public bool IsOnline { get; set; }

        public DateTime? LastSeenAt { get; set; }

        // Navigation

        public ICollection<UserConnection> Connections { get; set; } = new List<UserConnection>();

        public ICollection<GroupMember> GroupMemberships { get; set; } = new List<GroupMember>();

        public ICollection<Models.Group> CreatedGroups { get; set; } = new List<Models.Group>();

        public ICollection<GroupMessage> GroupMessages { get; set; } = new List<GroupMessage>();

        public ICollection<PrivateMessage> SentPrivateMessages { get; set; } = new List<PrivateMessage>();

        public ICollection<PrivateMessage> ReceivedPrivateMessages { get; set; } = new List<PrivateMessage>();
    }
}
