using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Authentications.Commands.ResetPassword
{

    public sealed class ResetPasswordCommandHandler
        : IRequestHandler<ResetPasswordCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordCommandHandler(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<string>> Handle(
            ResetPasswordCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

          
            if (user is null)
            {
                return Result<string>.Success(
                    "Password has been reset successfully.");
            }

            string decodedToken;

            try
            {
                decodedToken = Encoding.UTF8.GetString(
                    WebEncoders.Base64UrlDecode(request.Token));
            }
            catch
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Invalid reset token.");
            }

            var identityResult = await _userManager.ResetPasswordAsync(
                user,
                decodedToken,
                request.NewPassword);

            if (!identityResult.Succeeded)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    string.Join(
                        Environment.NewLine,
                        identityResult.Errors.Select(x => x.Description)));
            }

            return Result<string>.Success(
                "Password has been reset successfully.");
        }
    }
}
