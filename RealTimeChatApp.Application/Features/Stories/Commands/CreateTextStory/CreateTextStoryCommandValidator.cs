using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Commands.CreateTextStory
{
    public class CreateTextStoryCommandValidator
     : AbstractValidator<CreateTextStoryCommand>
    {
        public CreateTextStoryCommandValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Story content is required.")
                .MaximumLength(1000)
                .WithMessage("Story content cannot exceed 1000 characters.");

            RuleFor(x => x.BackgroundColor)
                .MaximumLength(20)
                .When(x => !string.IsNullOrWhiteSpace(x.BackgroundColor))
                .WithMessage("Background color is too long.");

            RuleFor(x => x.TextColor)
                .MaximumLength(20)
                .When(x => !string.IsNullOrWhiteSpace(x.TextColor))
                .WithMessage("Text color is too long.");

       
        }
    }
}
