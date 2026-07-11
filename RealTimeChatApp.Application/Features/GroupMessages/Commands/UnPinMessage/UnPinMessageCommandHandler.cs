using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.GroupMessages.Dtos;
using RealTimeChatApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.GroupMessages.Commands.UnPinMessage
{
    public class UnPinGroupMessageCommandHandler
     : IRequestHandler<
         UnPinGroupMessageCommand,
         Result<MessageUnPinnedNotifierDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public UnPinGroupMessageCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<MessageUnPinnedNotifierDto>> Handle(
            UnPinGroupMessageCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Result<MessageUnPinnedNotifierDto>.Failure(
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
                return Result<MessageUnPinnedNotifierDto>.Failure(
                    ResultStatus.NotFound,
                    "Message not found.");
            }

            var member = await _unitOfWork.GroupMembers
                .Query()
                .FirstOrDefaultAsync(
                    x => x.GroupId == message.GroupId &&
                         x.UserId == userId,
                    cancellationToken);

            if (member is null)
            {
                return Result<MessageUnPinnedNotifierDto>.Failure(
                    ResultStatus.Forbidden,
                    "You are not a member of this group.");
            }

            if (member.Role != GroupRole.Owner.ToString() &&
                member.Role != GroupRole.Admin.ToString())
            {
                return Result<MessageUnPinnedNotifierDto>.Failure(
                    ResultStatus.Forbidden,
                    "Only owner or admin can unpin messages.");
            }

            if (!message.IsPinned)
            {
                return Result<MessageUnPinnedNotifierDto>.Failure(
                    ResultStatus.Conflict,
                    "Message is not pinned.");
            }

            message.IsPinned = false;
            message.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveAsync();

            return Result<MessageUnPinnedNotifierDto>.Success(
                new MessageUnPinnedNotifierDto
                {
                    GroupId = message.GroupId,
                    MessageId = message.Id,
                    UnPinnedByUserId = userId
                });
        }
    }
}