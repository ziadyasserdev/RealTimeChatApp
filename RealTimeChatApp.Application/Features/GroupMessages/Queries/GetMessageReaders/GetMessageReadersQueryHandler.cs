using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.GroupMessages.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.GroupMessages.Queries.GetMessageReaders
{
    public class GetMessageReadersQueryHandler
      : IRequestHandler<
          GetMessageReadersQuery,
          Result<List<MessageReaderDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public GetMessageReadersQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<List<MessageReaderDto>>> Handle(
            GetMessageReadersQuery request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Result<List<MessageReaderDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized.");
            }

            var userId = _currentUser.UserId!;

            var message = await _unitOfWork.GroupMessages
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.MessageId,
                    cancellationToken);

            if (message is null)
            {
                return Result<List<MessageReaderDto>>.Failure(
                    ResultStatus.NotFound,
                    "Message not found.");
            }

            var isMember = await _unitOfWork.GroupMembers
                .Query()
                .AnyAsync(x =>
                    x.GroupId == message.GroupId &&
                    x.UserId == userId,
                    cancellationToken);

            if (!isMember)
            {
                return Result<List<MessageReaderDto>>.Failure(
                    ResultStatus.Forbidden,
                    "You are not a member of this group.");
            }

            var readers = await _unitOfWork.GroupMessageReads
                .Query()
                .AsNoTracking()
                .Include(x => x.User)
                .Where(x => x.GroupMessageId == request.MessageId)
                .OrderBy(x => x.ReadAt)
                .Select(x => new MessageReaderDto
                {
                    UserId = x.UserId,
                    UserName = x.User.UserName!,
                    ReadAt = x.ReadAt
                })
                .ToListAsync(cancellationToken);

            return Result<List<MessageReaderDto>>.Success(readers);
        }
    }
}
