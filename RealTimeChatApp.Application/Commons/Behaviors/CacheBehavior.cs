using MediatR;
using RealTimeChatApp.Application.Commons.Caching;
using RealTimeChatApp.Application.Contracts.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Commons.Behaviors
{
    public  class CacheBehavior<TRequest, TResponse>
       : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IRequest<TResponse>, ICacheable
    {
        private readonly ICacheService _cacheService;

        public CacheBehavior(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
           
            var cachedResponse = await _cacheService.GetAsync<TResponse>(
                request.CacheKey,
                cancellationToken);

            if (cachedResponse is not null)
            {
                return cachedResponse;
            }

          
            var response = await next();

         
            await _cacheService.SetAsync(
                request.CacheKey,
                response,
                request.CacheDuration,
                cancellationToken);

            return response;
        }
    }
}
