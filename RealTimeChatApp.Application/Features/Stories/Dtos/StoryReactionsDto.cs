using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Dtos
{
    public sealed class StoryReactionsDto
    {
        public int Count { get; set; }

        public IReadOnlyList<StoryReactionDto> Reactions { get; set; }
            = [];
    }
}
