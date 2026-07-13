using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Commands.CreateImageStory
{
    public class CreateImageStoryCommandValidator
     : AbstractValidator<CreateImageStoryCommand>
    {
        public CreateImageStoryCommandValidator()
        {
            RuleFor(x => x.Image)
                .NotNull()
                .WithMessage("Image is required.");

            RuleFor(x => x.Caption)
                .MaximumLength(500);
        }
    }
}
