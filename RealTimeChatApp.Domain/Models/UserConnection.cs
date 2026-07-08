using RealTimeChatApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Domain.Models
{
    public class UserConnection : BaseEntity
    {
        [ForeignKey("User")]
        public string UserId { get; set; } = null!;

        public string ConnectionId { get; set; } = null!;

        public DateTime ConnectedAt { get; set; } = DateTime.Now;

        public DateTime? DisconnectedAt { get; set; }

        public bool IsActive { get; set; }

        // Navigation

        public ApplicationUser User { get; set; } = null!;
    }
}
