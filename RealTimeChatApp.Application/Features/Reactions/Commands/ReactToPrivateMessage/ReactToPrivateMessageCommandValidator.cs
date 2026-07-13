using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Reactions.Commands.ReactToPrivateMessage
{
    public class ReactToPrivateMessageValidator
       : AbstractValidator<ReactToPrivateMessageCommand>
    {
        public ReactToPrivateMessageValidator()
        {
            RuleFor(x => x.MessageId)
                .GreaterThan(0);

            RuleFor(x => x.ReactionType)
                .IsInEnum();
        }
    }
}
