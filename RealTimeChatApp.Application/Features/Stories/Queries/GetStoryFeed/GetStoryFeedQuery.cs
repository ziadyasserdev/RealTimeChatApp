using MediatR;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Features.Stories.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Queries.GetStoryFeed
{
    public record GetStoryFeedQuery
     : IRequest<Result<List<StoryUserFeedDto>>>;
}
