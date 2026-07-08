using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealTimeChatApp.Application.Contracts.Services;
using RealTimeChatApp.Application.Features.Authentications.Dtos;
using RealTimeChatApp.Application.Settings;
using RealTimeChatApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly JwtSetting jwtSetting;
        private readonly RoleManager<IdentityRole> roleManager;
        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JwtSetting> options, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.jwtSetting = options.Value;
            this.roleManager = roleManager;
        }
        public async Task<AuthTokenDto> GenerateToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(JwtRegisteredClaimNames.Email,user.Email??""),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName??""),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName ?? ""),


            };

            var userCliams = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
                var roleName = await roleManager.FindByNameAsync(role);
                var roleCliams = await roleManager.GetClaimsAsync(roleName!);
                foreach (var claim in roleCliams)
                {
                    userCliams.Add(claim);
                }

            }
            claims.AddRange(userCliams);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtSetting.Issuer,
                audience: jwtSetting.Audience,
                claims: claims,
                signingCredentials: cred,
                expires: DateTime.UtcNow.AddMinutes(jwtSetting.DurationInMinutes)
                );
            var authToken = new AuthTokenDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                IsAuthenticated = true,
                Message = "Process Success"
                //Duration = jwtSetting.DurationInMinutes
            };
            return authToken;
        }
    }
}