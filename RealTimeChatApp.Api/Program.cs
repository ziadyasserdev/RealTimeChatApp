using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RealTimeChatApp.Api.Hubs;
using RealTimeChatApp.Api.Middleware;
using RealTimeChatApp.Api.SignalR.Services;
using RealTimeChatApp.Application.Extensions;
using RealTimeChatApp.Application.Settings;
using RealTimeChatApp.Infrastructure.Extensions;
using System.Text;
using System.Text.Json.Serialization;
namespace RealTimeChatApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
            });
            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.Configure<JwtSetting>(
         builder.Configuration.GetSection("JwtSetting")
     );

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateIssuerSigningKey = true,

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["JwtSetting:Issuer"],
                    ValidAudience = builder.Configuration["JwtSetting:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.
                    Configuration["JwtSetting:SecretKey"]))
                };

                o.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken)
                            && path.StartsWithSegments("/chatHub"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddSwaggerGen(options =>
            {
              
                options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme,
    securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization : `Bearer Genreated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = JwtBearerDefaults.AuthenticationScheme
            }
        },
        new string[] { }
    }
});

            });


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

            builder.Services.AddInfrastructureServices(builder.Configuration)
                .AddApplicationDependency();
            builder.Services.Configure<FileStorageSettings>(builder.Configuration.GetSection("FileStorageSettings"));

            builder.Services.AddSignalR();
            builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();
            app.UseCors("AllowAll");
            app.MapHub<ChatHub>("/ChatHub");

            app.MapControllers();
         

            app.Run();
        }
    }
}
