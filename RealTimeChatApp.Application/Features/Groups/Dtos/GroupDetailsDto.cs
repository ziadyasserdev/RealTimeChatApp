using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Dtos
{
    public class GroupDetailsDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsPrivate { get; set; }

        public int MembersCount { get; set; }

        public int MaxMembers { get; set; }

        public string CreatedById { get; set; } = default!;

        public string CreatedByName { get; set; } = default!;

        public DateTime CreatedAt { get; set; }

        public string? InviteCode { get; set; }
    }
}
