using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Commands.LeaveGroup
{
    public class LeaveGroupCommandValidator : AbstractValidator<LeaveGroupCommand>
    {
        public LeaveGroupCommandValidator()
        {
            RuleFor(x => x.GroupId)
                .GreaterThan(0);
        }
    }
}
