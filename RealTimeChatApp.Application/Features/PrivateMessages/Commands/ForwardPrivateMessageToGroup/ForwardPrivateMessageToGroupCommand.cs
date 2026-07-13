using MediatR;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Features.GroupMessages.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.PrivateMessages.Commands.ForwardPrivateMessageToGroup
{
    public record ForwardPrivateMessageToGroupCommand(
      int MessageId,
      int GroupId
  ) : IRequest<Result<GroupMessageNotifierDto>>;
}
