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

        public string MessageType { get; set; }

        public Guid? ReplyToMessageId { get; set; }

        public bool IsEdited { get; set; }

        public DateTime? EditedAt { get; set; }

        public DateTime SentAt { get; set; } = DateTime.Now;

        public bool DeletedForSender { get; set; }

        public bool DeletedForReceiver { get; set; }

        // Navigation

        public ApplicationUser Sender { get; set; } = null!;

        public ApplicationUser Receiver { get; set; } = null!;

        public ICollection<Reaction> Reactions { get; set; }
     = new List<Reaction>();
    }
}
