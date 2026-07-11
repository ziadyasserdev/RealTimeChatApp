using RealTimeChatApp.Domain.Enums;
using RealTimeChatApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Domain.Models
{
    public class GroupMessage : BaseEntity
    {
        [ForeignKey("Group")]
        public int GroupId { get; set; }
        [ForeignKey("Sender")]

        public string SenderId { get; set; } = null!;

        public string Content { get; set; } = null!;
         
        public MessageType MessageType { get; set; }


        public int? ReplyToMessageId { get; set; }

    

        public bool IsPinned { get; set; }

        public bool IsEdited { get; set; }

        public DateTime? EditedAt { get; set; }

        public DateTime SentAt { get; set; } = DateTime.Now ;

        // Navigation

        public Group Group { get; set; } = null!;
        public GroupMessage? ReplyToMessage { get; set; }
        public ApplicationUser Sender { get; set; } = null!;
      
        public ICollection<GroupMessageRead> Reads { get; set; } = new List<GroupMessageRead>();

        public ICollection<Reaction> Reactions { get; set; }
     = new List<Reaction>();

    }
}
