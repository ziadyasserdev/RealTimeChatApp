using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.PaginatedResults;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.PrivateMessages.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.PrivateMessages.Queries.GetConversation
{
    public class GetConversationHandler
    : IRequestHandler<
        GetConversationQuery,
        Result<PaginatedResult<PrivateMessageDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public GetConversationHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<PaginatedResult<PrivateMessageDto>>> Handle(
            GetConversationQuery request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Result<PaginatedResult<PrivateMessageDto>>
                    .Failure(ResultStatus.Unauthorized, "Unauthorized.");
            }

            var currentUserId = _currentUser.UserId!;

            var query = _unitOfWork.PrivateMessages
                .Query()
                .AsNoTracking()
                .Include(x => x.Sender)
                .Where(x =>

                    (
                        x.SenderId == currentUserId &&
                        x.ReceiverId == request.UserId &&
                        !x.DeletedForSender
                    )

                    ||

                    (

                        x.SenderId == request.UserId &&
                        x.ReceiverId == currentUserId &&
                        !x.DeletedForReceiver

                    )

                )
                .OrderByDescending(x => x.SentAt);

            var totalCount = await query.CountAsync(cancellationToken);

            var messages = await query

                .Skip((request.Page - 1) * request.PageSize)

                .Take(request.PageSize)

                .Select(x => new PrivateMessageDto
                {
                    Id = x.Id,

                    SenderId = x.SenderId,

                    SenderName = x.Sender.UserName!,

                    ReceiverId = x.ReceiverId,

                    Content = x.Content,

                    MessageType = x.MessageType,

                    IsEdited = x.IsEdited,

                    SentAt = x.SentAt,

                    ReplyToMessageId = x.ReplyToMessageId,

                    DeletedForMe =
                        x.SenderId == currentUserId
                        ? x.DeletedForSender
                        : x.DeletedForReceiver
                })

                .ToListAsync(cancellationToken);

            messages.Reverse();

            return Result<PaginatedResult<PrivateMessageDto>>
                .Success(
                    new PaginatedResult<PrivateMessageDto>(
                        messages,
                        totalCount,
                        request.Page,
                        request.PageSize));
        }
    }
}
