using RealTimeChatApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Domain.Models
{
    public class UserBlock : BaseEntity
    {
        [ForeignKey(nameof(Blocker))]
        public string BlockerId { get; set; } = null!;

        [ForeignKey(nameof(BlockedUser))]
        public string BlockedUserId { get; set; } = null!;

        public DateTime BlockedAt { get; set; }

        public ApplicationUser Blocker { get; set; } = null!;

        public ApplicationUser BlockedUser { get; set; } = null!;
    }
}
