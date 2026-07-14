using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Commands.ReactToStory
{
    public sealed class ReactToStoryCommandValidator : AbstractValidator<ReactToStoryCommand>
    {
        public ReactToStoryCommandValidator()
        {
            RuleFor(x => x.StoryId)
                .GreaterThan(0)
                .WithMessage("StoryId must be greater than 0.");

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Invalid reaction type.");
        }
    }
}
