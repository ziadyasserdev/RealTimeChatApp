using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Queries.GetStoryReactions
{

    public sealed class GetStoryReactionsQueryValidator
        : AbstractValidator<GetStoryReactionsQuery>
    {
        public GetStoryReactionsQueryValidator()
        {
            RuleFor(x => x.StoryId)
                .GreaterThan(0);
        }
    }
}
