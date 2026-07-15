using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.ExternalServices;
using RealTimeChatApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Authentications.Commands.ForgotPassword
{
    public sealed class ForgotPasswordCommandHandler
      : IRequestHandler<ForgotPasswordCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IEmailTemplateBuilder _emailTemplateBuilder;

        public ForgotPasswordCommandHandler(
            UserManager<ApplicationUser> userManager,
            IEmailService emailService,
            IEmailTemplateBuilder emailTemplateBuilder)
        {
            _userManager = userManager;
            _emailService = emailService;
            _emailTemplateBuilder = emailTemplateBuilder;
        }

        public async Task<Result<string>> Handle(
            ForgotPasswordCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

           
            if (user is null)
            {
                return Result<string>.Success(
                    "If the email exists, a password reset email has been sent.");
            }

           
            var token = await _userManager
                .GeneratePasswordResetTokenAsync(user);

         
            var encodedToken = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(token));

          
            var body = _emailTemplateBuilder.BuildPasswordResetTemplate(
                user.DisplayName,
                user.Email!,
                encodedToken);

          
            await _emailService.SendEmailAsync(
                user.Email!,
                "Reset Your Password",
                body);

            return Result<string>.Success(
                "If the email exists, a password reset email has been sent.");
        }
    }

}
