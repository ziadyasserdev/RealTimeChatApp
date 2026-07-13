using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.PrivateMessages.Commands.ForwardPrivateMessageToGroup
{
    public class ForwardPrivateMessageToGroupCommandValidator
     : AbstractValidator<ForwardPrivateMessageToGroupCommand>
    {
        public ForwardPrivateMessageToGroupCommandValidator()
        {
            RuleFor(x => x.MessageId)
                .GreaterThan(0);

            RuleFor(x => x.GroupId)
                .GreaterThan(0);
        }
    }
}
