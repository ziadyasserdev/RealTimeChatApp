using MediatR;
using RealTimeChatApp.Application.Commons.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Commands.ViewStory
{
    public record ViewStoryCommand(
    int StoryId
) : IRequest<Result<int>>;
}
