using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Queries.GetGroupMembers
{
    public class GetGroupMembersValidator
    : AbstractValidator<GetGroupMembersQuery>
    {
        public GetGroupMembersValidator()
        {
            RuleFor(x => x.GroupId)
                .GreaterThan(0);
        }
    }
}
