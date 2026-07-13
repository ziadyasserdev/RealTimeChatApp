using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Users.Commands.UnBlockUser
{
    public class UnBlockUserCommandValidator
     : AbstractValidator<UnBlockUserCommand>
    {
        public UnBlockUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
