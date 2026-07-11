using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.GroupMessages.Commands.EditGroupMessage
{
    public class EditGroupMessageCommandValidator
    : AbstractValidator<EditGroupMessageCommand>
    {
        public EditGroupMessageCommandValidator()
        {
            RuleFor(x => x.MessageId)
                .GreaterThan(0);

            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(2000);
        }
    }
}
