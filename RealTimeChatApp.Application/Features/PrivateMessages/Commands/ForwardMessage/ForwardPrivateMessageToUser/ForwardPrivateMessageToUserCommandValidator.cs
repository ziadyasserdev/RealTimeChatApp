using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.PrivateMessages.Commands.ForwardMessage.ForwardPrivateMessageToUser
{
    public sealed class ForwardPrivateMessageToUserCommandValidator
     : AbstractValidator<ForwardPrivateMessageToUserCommand>
    {
        public ForwardPrivateMessageToUserCommandValidator()
        {
            RuleFor(x => x.MessageId)
                .GreaterThan(0)
                .WithMessage("MessageId must be greater than 0.");

            RuleFor(x => x.ReceiverId)
                .NotEmpty()
                .WithMessage("ReceiverId is required.")
                .MaximumLength(450)
                .WithMessage("ReceiverId cannot exceed 450 characters.");
        }
    }
}
