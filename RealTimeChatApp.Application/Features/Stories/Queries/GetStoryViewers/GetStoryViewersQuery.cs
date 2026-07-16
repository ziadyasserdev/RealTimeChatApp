using MediatR;
using RealTimeChatApp.Application.Commons.Caching;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Features.Stories.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Queries.GetStoryViewers
{
    public class GetStoryViewersQuery : IRequest<Result<StoryViewersDto>>,
        ICacheable
    {
        public int StoryId { get; set; }
        public string UserId { get; set; }
        public string CacheKey =>
          CacheKeys.Stories.Viewers(StoryId, UserId);

        public TimeSpan CacheDuration =>
            TimeSpan.FromMinutes(2);
    }
   
}
