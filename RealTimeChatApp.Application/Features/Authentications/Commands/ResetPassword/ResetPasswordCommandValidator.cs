using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RealTimeChatApp.Application.Features.Authentications.Commands.ResetPassword
{

    public sealed class ResetPasswordCommandValidator
        : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Token)
                .NotEmpty()
                .WithMessage("Reset token is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(8)
                .Matches("[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]")
                .WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]")
                .WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]")
                .WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword)
                .WithMessage("Passwords do not match.");
        }
    }
}
