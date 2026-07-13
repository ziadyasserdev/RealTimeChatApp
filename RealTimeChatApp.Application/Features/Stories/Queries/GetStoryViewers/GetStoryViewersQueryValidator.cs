using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Queries.GetStoryViewers
{
    public class GetStoryViewersQueryValidator
     : AbstractValidator<GetStoryViewersQuery>
    {
        public GetStoryViewersQueryValidator()
        {
            RuleFor(x => x.StoryId)
                .GreaterThan(0)
                .WithMessage("StoryId is required.");
        }
    }
}
