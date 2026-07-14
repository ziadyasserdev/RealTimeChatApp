using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Infrastructure.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }
        public string? UserId => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        public string? UserName =>
       httpContextAccessor.HttpContext?
           .User?
           .Identity?
           .Name;

        public string? Email => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

        public bool IsAuthenticated => httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public IEnumerable<string> Roles => httpContextAccessor.HttpContext?.User?.Claims
           .Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value) ?? Enumerable.Empty<string>();

        public bool IsInRole(string role)
        => Roles != null && Roles.Contains(role, StringComparer.OrdinalIgnoreCase);
    }
}
