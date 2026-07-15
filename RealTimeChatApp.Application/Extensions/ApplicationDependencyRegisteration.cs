using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RealTimeChatApp.Application.Commons.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Extensions
{
    public static class ApplicationDependencyRegisteration
    {

        public static IServiceCollection AddApplicationDependency(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(CacheInvalidationBehavior<,>));

            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(CacheBehavior<,>));
            return services;
        }
    }

}
