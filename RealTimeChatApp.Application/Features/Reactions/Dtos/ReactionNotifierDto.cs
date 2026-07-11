using RealTimeChatApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Reactions.Dtos
{
    public class ReactionNotifierDto
    {
        public int GroupId { get; set; }

        public int MessageId { get; set; }

        public string UserId { get; set; } = default!;

        public string UserName { get; set; } = default!;

        public ReactionType ReactionType { get; set; }

        public bool Removed { get; set; }
    }
}
