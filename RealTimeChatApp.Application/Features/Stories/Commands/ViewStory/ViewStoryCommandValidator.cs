using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Commands.ViewStory
{
    public class ViewStoryCommandValidator : AbstractValidator<ViewStoryCommand>
    {
        public ViewStoryCommandValidator()
        {
            RuleFor(x => x.StoryId)
                .GreaterThan(0)
                .WithMessage("Story id must be greater than zero.");
        }
    }
}
