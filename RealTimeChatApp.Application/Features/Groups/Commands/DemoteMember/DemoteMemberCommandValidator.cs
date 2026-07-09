using FluentValidation;
using RealTimeChatApp.Application.Features.Groups.Commands.PromoteMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Commands.DemoteMember
{
    public class DemoteMemberCommandValidator
     : AbstractValidator<DemoteMemberCommand>
    {
        public DemoteMemberCommandValidator()
        {
            RuleFor(x => x.GroupId)
                .GreaterThan(0);

            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
