using MediatR;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Features.Reactions.Dtos;
using RealTimeChatApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Reactions.Commands.ReactToPrivateMessage
{
    public record ReactToPrivateMessageCommand(
     int MessageId,
     ReactionType ReactionType
 ) : IRequest<Result<ReactionNotifierDto>>;
}
