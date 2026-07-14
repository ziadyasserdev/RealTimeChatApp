using MediatR;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Features.Stories.Dtos;
using RealTimeChatApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Commands.ReactToStory
{
    public record ReactToStoryCommand(
     int StoryId,
     ReactionType Type
 ) : IRequest<Result<StoryReactionNotifierDto>>;
}
