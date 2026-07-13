using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Dtos
{
    public class StoryViewersDto
    {
        public int ViewsCount { get; set; }

        public List<StoryViewerDto> Viewers { get; set; } = [];
    }
}
