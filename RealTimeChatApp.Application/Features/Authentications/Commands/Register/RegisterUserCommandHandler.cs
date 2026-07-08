using MediatR;
using Microsoft.AspNetCore.Identity;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Authentications.Commands.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Result<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await userManager.FindByEmailAsync(request.Email) != null)
           return Result<string>.Failure(ResultStatus.Failure,"Email is already registered");
            var user = new ApplicationUser
            {
                UserName = request.DisplayName,
                Email = request.Email,
                DisplayName = request.DisplayName,
               
                Bio = request.Bio,
                IsOnline = false,
                LastSeenAt = null
            };
            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<string>.Failure(ResultStatus.Failure, $"User registration failed: {errors}");
            }
            return Result<string>.Success("User registered successfully");
        }
    }
}
