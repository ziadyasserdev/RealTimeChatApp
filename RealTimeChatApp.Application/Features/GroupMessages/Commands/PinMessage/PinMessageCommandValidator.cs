using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.GroupMessages.Commands.PinMessage
{
    public class PinGroupMessageValidator
      : AbstractValidator<PinGroupMessageCommand>
    {
        public PinGroupMessageValidator()
        {
            RuleFor(x => x.MessageId)
                .GreaterThan(0);
        }
    }
}
