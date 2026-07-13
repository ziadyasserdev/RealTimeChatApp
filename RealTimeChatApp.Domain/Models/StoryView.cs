using RealTimeChatApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Domain.Models
{
    public class StoryView : BaseEntity
    {
        public int StoryId { get; set; }

        public string ViewerId { get; set; } = null!;


        public DateTime ViewedAt { get; set; }


        public Story Story { get; set; } = null!;

        public ApplicationUser Viewer { get; set; } = null!;
    }
}
