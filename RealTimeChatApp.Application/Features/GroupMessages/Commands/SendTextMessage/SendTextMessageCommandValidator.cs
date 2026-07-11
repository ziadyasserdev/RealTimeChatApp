using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.GroupMessages.Commands.SendTextMessage
{
    public class SendTextMessageCommandValidator
     : AbstractValidator<SendTextMessageCommand>
    {
        public SendTextMessageCommandValidator()
        {
            RuleFor(x => x.GroupId)
                .GreaterThan(0);

            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(2000);

            RuleFor(x => x.Content)
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Message cannot be empty.");
        }
    }
}
