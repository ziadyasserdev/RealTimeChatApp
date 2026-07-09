using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Queries.GetGroupDetails
{
    public class GetGroupDetailsValidator
     : AbstractValidator<GetGroupDetailsQuery>
    {
        public GetGroupDetailsValidator()
        {
            RuleFor(x => x.GroupId)
                .GreaterThan(0);
        }
    }
}
