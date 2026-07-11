using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.PrivateMessages.Commands.SendTextMessage
{
    public class SendPrivateMessageValidator
    : AbstractValidator<SendPrivateMessageCommand>
    {
        public SendPrivateMessageValidator()
        {
            RuleFor(x => x.ReceiverId)
                .NotEmpty();

            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(2000);
        }
    }
}
