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
    public class PrivateMessage : BaseEntity
    {
        [ForeignKey("Sender")]  
        public string SenderId { get; set; } = null!;
        [ForeignKey("Receiver")]

        public string ReceiverId { get; set; } = null!;

        public string Content { get; set; } = null!;
        public bool IsDeletedForEveryone { get; set; }

        public DateTime? DeletedAt { get; set; }

        public MessageType MessageType { get; set; }


        public bool IsEdited { get; set; }

        public DateTime? EditedAt { get; set; }

        public DateTime SentAt { get; set; } = DateTime.Now;

        public bool DeletedForSender { get; set; }
        public bool IsForwarded { get; set; }

        public string? ForwardedFromUserId { get; set; }

      
        public bool DeletedForReceiver { get; set; }
        public bool IsRead { get; set; }

        public DateTime? ReadAt { get; set; }
        // Navigation
        public int? ReplyToMessageId { get; set; }

        public PrivateMessage? ReplyToMessage { get; set; }
        public ApplicationUser Sender { get; set; } = null!;
        public ApplicationUser? ForwardedFromUser { get; set; }

        public ApplicationUser Receiver { get; set; } = null!;

        public ICollection<Reaction> Reactions { get; set; }
     = new List<Reaction>();
    }
}
