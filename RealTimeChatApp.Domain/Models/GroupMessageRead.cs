using RealTimeChatApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Domain.Models
{
    public class GroupMessageRead : BaseEntity
    {
        [ForeignKey("GroupMessage")]
        public int GroupMessageId { get; set; }
        [ForeignKey("User")]

        public string UserId { get; set; } = null!;

        public DateTime ReadAt { get; set; } = DateTime.Now;

        // Navigation

        public GroupMessage GroupMessage { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;
    }
}
