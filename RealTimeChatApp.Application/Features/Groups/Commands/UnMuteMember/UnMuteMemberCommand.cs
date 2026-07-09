using MediatR;
using RealTimeChatApp.Application.Commons.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Commands.UnMuteMember
{
    public record UnMuteMemberCommand(
        int GroupId,
        string UserId
    ) : IRequest<Result<string>>;
}
