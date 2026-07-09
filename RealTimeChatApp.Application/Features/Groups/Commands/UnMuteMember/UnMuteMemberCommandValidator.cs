using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Commands.UnMuteMember
{
    public class UnMuteMemberCommandValidator
         : AbstractValidator<UnMuteMemberCommand>
    {
        public UnMuteMemberCommandValidator()
        {
            RuleFor(x => x.GroupId)
                .GreaterThan(0);

            RuleFor(x => x.UserId)
                .NotEmpty()
                .MaximumLength(450);
        }
    }
}
