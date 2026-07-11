using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.GroupMessages.Dtos;
using RealTimeChatApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.GroupMessages.Commands.MarkAsRead
{
    public class MarkGroupMessageAsReadCommandHandler
    : IRequestHandler<
        MarkGroupMessageAsReadCommand,
        Result<MessageReadNotifierDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public MarkGroupMessageAsReadCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<MessageReadNotifierDto>> Handle(
            MarkGroupMessageAsReadCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Result<MessageReadNotifierDto>.Failure(
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
                return Result<MessageReadNotifierDto>.Failure(
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
                return Result<MessageReadNotifierDto>.Failure(
                    ResultStatus.Forbidden,
                    "You are not a member of this group.");
            }

            var alreadyRead = await _unitOfWork.GroupMessageReads
                .Query()
                .AnyAsync(
                    x => x.GroupMessageId == request.MessageId &&
                         x.UserId == userId,
                    cancellationToken);

            if (!alreadyRead)
            {
                await _unitOfWork.GroupMessageReads.AddAsync(
                    new GroupMessageRead
                    {
                        GroupMessageId = request.MessageId,
                        UserId = userId,
                        ReadAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    });

                

                await _unitOfWork.SaveAsync();
            }

            return Result<MessageReadNotifierDto>.Success(
                new MessageReadNotifierDto
                {
                    GroupId = message.GroupId,
                    MessageId = message.Id,
                    UserId = userId,
                    ReadAt = DateTime.UtcNow
                });
        }
    }
}
