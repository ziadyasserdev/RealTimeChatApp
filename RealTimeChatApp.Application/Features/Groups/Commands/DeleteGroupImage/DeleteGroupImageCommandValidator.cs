using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Commands.DeleteGroupImage
{
    public class DeleteGroupImageCommandValidator
       : AbstractValidator<DeleteGroupImageCommand>
    {
        public DeleteGroupImageCommandValidator()
        {
            RuleFor(x => x.GroupId)
                .GreaterThan(0)
                .WithMessage("Invalid group id.");
        }
    }
}
