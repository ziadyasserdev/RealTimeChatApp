using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.PrivateMessages.Queries.SearchPrivateMessages
{
    public class SearchPrivateMessagesQueryValidator
   : AbstractValidator<SearchPrivateMessagesQuery>
    {
        public SearchPrivateMessagesQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.")
                .MaximumLength(450)
                .WithMessage("UserId cannot exceed 450 characters.");

            RuleFor(x => x.Keyword)
                .NotEmpty()
                .WithMessage("Keyword is required.")
                .MinimumLength(2)
                .WithMessage("Keyword must be at least 2 characters.")
                .MaximumLength(100)
                .WithMessage("Keyword cannot exceed 100 characters.");
        }
    }
}
