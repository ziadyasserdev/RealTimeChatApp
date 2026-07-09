using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Commands.ChangeAnnouncementMode
{
    public class ChangeAnnouncementModeCommandValidator
         : AbstractValidator<ChangeAnnouncementModeCommand>
    {
        public ChangeAnnouncementModeCommandValidator()
        {
            RuleFor(x => x.GroupId)
                .GreaterThan(0)
                .WithMessage("Invalid group id.");
        }
    }
}
