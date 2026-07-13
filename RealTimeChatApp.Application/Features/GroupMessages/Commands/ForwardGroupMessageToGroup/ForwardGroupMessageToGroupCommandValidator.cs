using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.GroupMessages.Commands.ForwardGroupMessageToGroup
{
    public sealed class ForwardGroupMessageToGroupCommandValidator
    : AbstractValidator<ForwardGroupMessageToGroupCommand>
    {
        public ForwardGroupMessageToGroupCommandValidator()
        {
            RuleFor(x => x.MessageId)
                .GreaterThan(0)
                .WithMessage("MessageId must be greater than 0.");

            RuleFor(x => x.GroupId)
                .GreaterThan(0)
                .WithMessage("GroupId must be greater than 0.");
        }
    }
}
