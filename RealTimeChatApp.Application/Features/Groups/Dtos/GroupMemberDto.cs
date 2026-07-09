using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Dtos
{
    public class GroupMemberDto
    {
        public string UserId { get; set; } = default!;

        public string UserName { get; set; } = default!;

        public string? Nickname { get; set; }

        public string Role { get; set; } = default!;

        public bool IsMuted { get; set; }

        public DateTime JoinedAt { get; set; }
    }
}
