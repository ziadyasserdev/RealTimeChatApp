using MediatR;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Features.PrivateMessages.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.PrivateMessages.Queries.SearchPrivateMessages
{
    public record SearchPrivateMessagesQuery(
    string UserId,
    string Keyword
) : IRequest<Result<List<PrivateMessageSearchDto>>>;
}
