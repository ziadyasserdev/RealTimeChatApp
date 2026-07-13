using MediatR;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Features.PrivateMessages.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.GroupMessages.Commands.ForwardGroupMessageToUser
{
    public record ForwardGroupMessageToUserCommand(
     int MessageId,
     string ReceiverId
 ) : IRequest<Result<PrivateMessageNotifierDto>>;
}
