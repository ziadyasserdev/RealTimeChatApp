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
    public sealed class CacheInvalidationBehavior<TRequest, TResponse>
       : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IRequest<TResponse>
    {
        private readonly ICacheService _cacheService;

        public CacheInvalidationBehavior(
            ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var response = await next();

            if (request is IInvalidateCache invalidateCache)
            {
                foreach (var key in invalidateCache.CacheKeysToInvalidate)
                {
                    await _cacheService.RemoveAsync(
                        key,
                        cancellationToken);
                }
            }

            return response;
        }
    }
}
