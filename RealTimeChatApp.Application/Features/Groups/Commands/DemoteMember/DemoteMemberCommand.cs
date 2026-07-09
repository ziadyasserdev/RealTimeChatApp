using MediatR;
using RealTimeChatApp.Application.Commons.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Commands.DemoteMember
{
    public record DemoteMemberCommand(
      int GroupId,
      string UserId)
      : IRequest<Result<string>>;
}
