using MediatR;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Features.Stories.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Queries.GetStoryReactions
{

    public record GetStoryReactionsQuery(
        int StoryId
    ) : IRequest<Result<StoryReactionsDto>>;
}
