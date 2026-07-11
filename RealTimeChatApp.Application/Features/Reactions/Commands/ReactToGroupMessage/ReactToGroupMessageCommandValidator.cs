using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Reactions.Commands.ReactToGroupMessage
{
    public class ReactToGroupMessageValidator
    : AbstractValidator<ReactToGroupMessageCommand>
    {
        public ReactToGroupMessageValidator()
        {
            RuleFor(x => x.MessageId)
                .GreaterThan(0);

            RuleFor(x => x.ReactionType)
                .IsInEnum();
        }
    }
}
