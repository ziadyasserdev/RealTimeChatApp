using MediatR;
using RealTimeChatApp.Application.Commons.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Users.Commands.UnBlockUser
{
    public record UnBlockUserCommand(
     string UserId
 ) : IRequest<Result<string>>;
}
