using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.GroupMessages.Dtos;
using RealTimeChatApp.Domain.Enums;
using RealTimeChatApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.GroupMessages.Commands.SendTextMessage
{
    public class SendTextMessageCommandHandler
      : IRequestHandler<SendTextMessageCommand, Result<GroupMessageNotifierDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public SendTextMessageCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<GroupMessageNotifierDto>> Handle(
            SendTextMessageCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Result<GroupMessageNotifierDto>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            var userId = _currentUser.UserId!;

            var member = await _unitOfWork.GroupMembers
                .Query()
                .FirstOrDefaultAsync(x =>
                    x.GroupId == request.GroupId &&
                    x.UserId == userId,
                    cancellationToken);

            if (member is null)
            {
                return Result<GroupMessageNotifierDto>.Failure(
                    ResultStatus.Forbidden,
                    "You are not a member of this group.");
            }

            var group = await _unitOfWork.Groups
                .GetByIdAsync(request.GroupId);

            if (group is null)
            {
                return Result<GroupMessageNotifierDto>.Failure(
                    ResultStatus.NotFound,
                    "Group not found.");
            }

            if (group.OnlyAdminsCanSendMessages &&
                member.Role != GroupRole.Owner.ToString() &&
                member.Role != GroupRole.Admin.ToString())
            {
                return Result<GroupMessageNotifierDto>.Failure(
                    ResultStatus.Forbidden,
                    "Only admins can send messages.");
            }

            if (member.IsMuted &&
                member.MutedUntil.HasValue &&
                member.MutedUntil > DateTime.UtcNow)
            {
                return Result<GroupMessageNotifierDto>.Failure(
                    ResultStatus.Forbidden,
                    "You are muted.");
            }

            var message = new GroupMessage
            {
                GroupId = request.GroupId,
                SenderId = userId,
                Content = request.Content,
                MessageType = MessageType.Text,
                SentAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                IsEdited = false,
                IsPinned = false
            };

            await _unitOfWork.GroupMessages.AddAsync(message);

            await _unitOfWork.SaveAsync();

            var dto = new GroupMessageNotifierDto
            {
                Id = message.Id,
                GroupId = message.GroupId,
                SenderId = message.SenderId,
                SenderName = _currentUser.UserName!,
                Content = message.Content,
                MessageType = message.MessageType.ToString(),
                SentAt = message.SentAt,
                IsEdited = message.IsEdited,
                EditedAt = message.EditedAt,
                IsPinned = message.IsPinned
            };

            return Result<GroupMessageNotifierDto>.Success(dto);
        }
    }
}
