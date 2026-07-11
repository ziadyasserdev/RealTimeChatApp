using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.GroupMessages.Commands.DeleteMessage
{
    public class DeleteGroupMessageValidator
       : AbstractValidator<DeleteGroupMessageCommand>
    {
        public DeleteGroupMessageValidator()
        {
            RuleFor(x => x.MessageId)
                .GreaterThan(0);
        }
    }
}
