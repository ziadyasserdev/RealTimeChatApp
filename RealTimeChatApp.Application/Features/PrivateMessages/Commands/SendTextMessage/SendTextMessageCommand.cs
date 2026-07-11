using MediatR;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Features.PrivateMessages.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.PrivateMessages.Commands.SendTextMessage
{
    public record SendPrivateMessageCommand(
     string ReceiverId,
     string Content,
     int? ReplyToMessageId
 ) : IRequest<Result<PrivateMessageNotifierDto>>;
}
