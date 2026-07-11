using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.GroupMessages.Commands.UnPinMessage
{
    public class UnPinGroupMessageValidator
    : AbstractValidator<UnPinGroupMessageCommand>
    {
        public UnPinGroupMessageValidator()
        {
            RuleFor(x => x.MessageId)
                .GreaterThan(0);
        }
    }
}
