using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.GroupMessages.Queries.SearchGroupMessages
{
    public  class SearchGroupMessagesQueryValidator
     : AbstractValidator<SearchGroupMessagesQuery>
    {
        public SearchGroupMessagesQueryValidator()
        {
            RuleFor(x => x.GroupId)
                .GreaterThan(0)
                .WithMessage("GroupId must be greater than 0.");

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
