using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.PaginatedResults;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.Groups.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Queries.GetGroupMessages
{
    public class GetGroupMessagesHandler
    : IRequestHandler<GetGroupMessagesQuery,
        Result<PaginatedResult<GroupMessagesDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public GetGroupMessagesHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<PaginatedResult<GroupMessagesDto>>> Handle(
            GetGroupMessagesQuery request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
                return Result<PaginatedResult<GroupMessagesDto>>
                    .Failure(ResultStatus.Unauthorized, "Unauthorized.");

            var userId = _currentUser.UserId!;

            var isMember = await _unitOfWork.GroupMembers.Query()

                .AnyAsync(x =>
                    x.GroupId == request.GroupId &&
                    x.UserId == userId,
                    cancellationToken);

            if (!isMember)
                return Result<PaginatedResult<GroupMessagesDto>>
                    .Failure(ResultStatus.Forbidden,
                        "You are not a member of this group.");

            var query = _unitOfWork.GroupMessages.Query()

                .AsNoTracking()

                .Where(x => x.GroupId == request.GroupId)

                .Include(x => x.Sender)

                .OrderByDescending(x => x.SentAt);

            var totalCount = await query.CountAsync(cancellationToken);

            var messages = await query

                .Skip((request.Page - 1) * request.PageSize)

                .Take(request.PageSize)

                .Select(x => new GroupMessagesDto
                {
                    Id = x.Id,
                    SenderId = x.SenderId,
                    SenderName = x.Sender.UserName!,
                    Content = x.Content,
                    MessageType = x.MessageType.ToString(),
                    IsEdited = x.IsEdited,
                    IsPinned = x.IsPinned,
                    SentAt = x.SentAt
                })

                .ToListAsync(cancellationToken);

            messages.Reverse();

            return Result<PaginatedResult<GroupMessagesDto>>
                .Success(
                    new PaginatedResult<GroupMessagesDto>(
                        messages,
                        totalCount,
                        request.Page,
                        request.PageSize));
        }
    }
}
