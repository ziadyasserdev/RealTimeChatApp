using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.GroupMessages.Commands.MarkAsRead
{
    public class MarkGroupMessageAsReadValidator
    : AbstractValidator<MarkGroupMessageAsReadCommand>
    {
        public MarkGroupMessageAsReadValidator()
        {
            RuleFor(x => x.MessageId)
                .GreaterThan(0);
        }
    }
}
