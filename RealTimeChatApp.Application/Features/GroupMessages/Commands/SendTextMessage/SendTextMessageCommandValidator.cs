using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.GroupMessages.Commands.SendTextMessage
{

    public class SendTextMessageValidator
        : AbstractValidator<SendTextMessageCommand>
    {
        public SendTextMessageValidator()
        {
            RuleFor(x => x.GroupId)
                .GreaterThan(0);

            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(2000);

            RuleFor(x => x.ReplyToMessageId)
                .GreaterThan(0)
                .When(x => x.ReplyToMessageId.HasValue);
        }
    }
}
