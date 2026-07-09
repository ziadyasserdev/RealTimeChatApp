using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Commands.MuteMember
{
    public class MuteMemberCommandValidator : AbstractValidator<MuteMemberCommand>
    {
        public MuteMemberCommandValidator()
        {
            RuleFor(x => x.GroupId)
                .GreaterThan(0);

            RuleFor(x => x.UserId)
                .NotEmpty()
                .MaximumLength(450);

            RuleFor(x => x.MutedUntil)
                .Must(date => !date.HasValue || date.Value > DateTime.UtcNow)
                .WithMessage("MutedUntil must be a future date.");
        }
    }
}
