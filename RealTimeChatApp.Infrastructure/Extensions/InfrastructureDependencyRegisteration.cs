using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealTimeChatApp.Application.Contracts.Services;
using RealTimeChatApp.Domain.Identity;
using RealTimeChatApp.Infrastructure.Persistence.Context;
using RealTimeChatApp.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Infrastructure.Extensions
{
    public static class InfrastructureDependencyRegisteration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DbConn")));


            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
           ;
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<ICurrentUserService, CurrentUserService>();
            //// تسجيل IHttpContextAccessor
            //services.AddScoped<IFileService, FileService>();
            //services.AddScoped<IMediaService, MediaService>();
            //services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthService, AuthService>();
            //services.AddScoped<IVideoProcessingService, VideoProcessingService>();
            return services;
        }
    }
}
