using RealTimeChatApp.Application.Features.Authentications.Dtos;
using RealTimeChatApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Contracts.Services
{
    public interface IAuthService
    {
        public Task<AuthTokenDto> GenerateToken(ApplicationUser user);
      
    }
}
