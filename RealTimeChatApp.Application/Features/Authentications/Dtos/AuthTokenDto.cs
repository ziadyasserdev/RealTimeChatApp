using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Authentications.Dtos
{
    public class AuthTokenDto
    {
        public string? Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        [JsonIgnore]
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiration { get; set; }

       


    }

}
