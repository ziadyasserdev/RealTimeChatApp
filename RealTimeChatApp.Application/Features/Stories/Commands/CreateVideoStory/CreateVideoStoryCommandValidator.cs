using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Commands.CreateVideoStory
{
    public class CreateVideoStoryCommandValidator
    : AbstractValidator<CreateVideoStoryCommand>
    {
        public CreateVideoStoryCommandValidator()
        {
            RuleFor(x => x.Video)
                .NotNull()
                .WithMessage("Video is required.");

            RuleFor(x => x.Caption)
                .MaximumLength(500);
        }
    }
}
