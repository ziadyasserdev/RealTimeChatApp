using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.PrivateMessages.Commands.MarkAsRead
{

    public class MarkPrivateMessageAsReadValidator
        : AbstractValidator<MarkPrivateMessageAsReadCommand>
    {
        public MarkPrivateMessageAsReadValidator()
        {
            RuleFor(x => x.MessageId)
                .GreaterThan(0);
        }
    }
}
