using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Commands.RemoveMember
{
    public class RemoveMemberValidator
     : AbstractValidator<RemoveMemberCommand>
    {
        public RemoveMemberValidator()
        {
            RuleFor(x => x.GroupId)
                .GreaterThan(0);

            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
