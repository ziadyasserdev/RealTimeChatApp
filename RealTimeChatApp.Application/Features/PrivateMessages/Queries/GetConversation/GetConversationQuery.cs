using MediatR;
using RealTimeChatApp.Application.Commons.PaginatedResults;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Features.PrivateMessages.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.PrivateMessages.Queries.GetConversation
{
    public record GetConversationQuery(
     string UserId,
     int Page = 1,
     int PageSize = 20
 ) : IRequest<Result<PaginatedResult<PrivateMessageDto>>>;
}
