using MediatR;
using RealTimeChatApp.Application.Commons.Caching;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Features.Stories.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Queries.GetStoryFeed
{
    public class GetStoryFeedQuery
     : IRequest<Result<List<StoryUserFeedDto>>>, ICacheable
    {
      public string UserId { get; set; }

        public string CacheKey =>
          CacheKeys.Stories.Feed(UserId);

        public TimeSpan CacheDuration =>
            TimeSpan.FromMinutes(2);
    }
}
