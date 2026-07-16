using RealTimeChatApp.Application.Contracts.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Commons.Caching
{
    public interface ICacheable

    {
      
        string CacheKey { get; }
        TimeSpan CacheDuration { get; }
    }
}
