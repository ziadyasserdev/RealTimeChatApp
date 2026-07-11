using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.PrivateMessages.Dtos;
using RealTimeChatApp.Domain.Enums;
using RealTimeChatApp.Domain.Identity;
using RealTimeChatApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.PrivateMessages.Commands.SendTextMessage
{
    public class SendPrivateMessageCommandHandler
     : IRequestHandler<SendPrivateMessageCommand, Result<PrivateMessageNotifierDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<ApplicationUser> _userManager;

        public SendPrivateMessageCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _userManager = userManager;
        }

        public async Task<Result<PrivateMessageNotifierDto>> Handle(
            SendPrivateMessageCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Result<PrivateMessageNotifierDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized.");
            }

            var senderId = _currentUser.UserId!;

            if (senderId == request.ReceiverId)
            {
                return Result<PrivateMessageNotifierDto>.Failure(
                    ResultStatus.Failure,
                    "You cannot send a message to yourself.");
            }

            var receiver = await _userManager.FindByIdAsync(request.ReceiverId);

            if (receiver is null)
            {
                return Result<PrivateMessageNotifierDto>.Failure(
                    ResultStatus.NotFound,
                    "Receiver not found.");
            }

            if (request.ReplyToMessageId.HasValue)
            {
                var replyExists = await _unitOfWork.PrivateMessages
                    .Query()
                    .AnyAsync(x =>
                        x.Id == request.ReplyToMessageId,
                        cancellationToken);

                if (!replyExists)
                {
                    return Result<PrivateMessageNotifierDto>.Failure(
                        ResultStatus.NotFound,
                        "Reply message not found.");
                }
            }

            var message = new PrivateMessage
            {
                SenderId = senderId,
                ReceiverId = request.ReceiverId,
                Content = request.Content,
                MessageType = MessageType.Text,
                ReplyToMessageId = request.ReplyToMessageId,
                SentAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                IsEdited = false,
                DeletedForReceiver = false,
                DeletedForSender = false
            };

            await _unitOfWork.PrivateMessages.AddAsync(message);

            await _unitOfWork.SaveAsync();

            var dto = new PrivateMessageNotifierDto
            {
                MessageId = message.Id,
                SenderId = senderId,
                SenderName = _currentUser.UserName!,
                ReceiverId = request.ReceiverId,
                Content = message.Content,
                MessageType = message.MessageType,
                ReplyToMessageId = message.ReplyToMessageId,
                SentAt = message.SentAt,
                IsEdited = false
            };

            return Result<PrivateMessageNotifierDto>.Success(dto);
        }
    }
}
