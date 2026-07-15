using MediatR;
using Microsoft.AspNetCore.Identity;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Authentications.Commands.ChangePassword
{
    public sealed class ChangePasswordCommandHandler
        : IRequestHandler<ChangePasswordCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public ChangePasswordCommandHandler(
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            ChangePasswordCommand request,
            CancellationToken cancellationToken)
        {
           if(!_currentUserService.IsAuthenticated)
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            var userId = _currentUserService.UserId;
            if(userId is null)
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "User not found.");
            }

         
            var isCurrentPasswordValid =
                await _userManager.CheckPasswordAsync(
                    user,
                    request.CurrentPassword);

            if (!isCurrentPasswordValid)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Current password is incorrect.");
            }

           
            var isSamePassword =
                await _userManager.CheckPasswordAsync(
                    user,
                    request.NewPassword);

            if (isSamePassword)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "The new password must be different from the current password.");
            }

          
            var identityResult =
                await _userManager.ChangePasswordAsync(
                    user,
                    request.CurrentPassword,
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
                "Password changed successfully.");
        }
    }
}

