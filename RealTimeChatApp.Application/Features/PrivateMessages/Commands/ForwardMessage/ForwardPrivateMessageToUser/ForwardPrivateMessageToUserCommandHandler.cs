using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.PrivateMessages.Dtos;
using RealTimeChatApp.Domain.Identity;
using RealTimeChatApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.PrivateMessages.Commands.ForwardMessage.ForwardPrivateMessageToUser
{
    public class ForwardPrivateMessageToUserCommandHandler
      : IRequestHandler<
          ForwardPrivateMessageToUserCommand,
          Result<PrivateMessageNotifierDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<ApplicationUser> _userManager;

        public ForwardPrivateMessageToUserCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _userManager = userManager;
        }

        public async Task<Result<PrivateMessageNotifierDto>> Handle(
            ForwardPrivateMessageToUserCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Result<PrivateMessageNotifierDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized.");
            }

            var currentUserId = _currentUser.UserId!;

            var originalMessage = await _unitOfWork.PrivateMessages
                .Query()
                .Include(x => x.Sender)
                .FirstOrDefaultAsync(
                    x => x.Id == request.MessageId,
                    cancellationToken);

            if (originalMessage is null)
            {
                return Result<PrivateMessageNotifierDto>.Failure(
                    ResultStatus.NotFound,
                    "Message not found.");
            }

            var receiver = await _userManager.Users
                .FirstOrDefaultAsync(
                    x => x.Id == request.ReceiverId,
                    cancellationToken);

            if (receiver is null)
            {
                return Result<PrivateMessageNotifierDto>.Failure(
                    ResultStatus.NotFound,
                    "Receiver not found.");
            }

            var forwardedMessage = new PrivateMessage
            {
                SenderId = currentUserId,

                ReceiverId = receiver.Id,

                Content = originalMessage.Content,

                MessageType = originalMessage.MessageType,

                ReplyToMessageId = null,

                SentAt = DateTime.UtcNow,

                IsEdited = false,

                IsRead = false,

                DeletedForReceiver = false,

                DeletedForSender = false,

                IsDeletedForEveryone = false,

                IsForwarded = true,

                ForwardedFromUserId = originalMessage.SenderId
            };

            await _unitOfWork.PrivateMessages
                .AddAsync(forwardedMessage);

            await _unitOfWork.SaveAsync();

            var dto = new PrivateMessageNotifierDto
            {
                MessageId = forwardedMessage.Id,

                SenderId = currentUserId,

                SenderName = _currentUser.UserName!,

                ReceiverId = receiver.Id,

                Content = forwardedMessage.Content,

                MessageType = forwardedMessage.MessageType,

                IsEdited = false,

                SentAt = forwardedMessage.SentAt,

                IsRead = false,

                ReadAt = null,

                ReplyToMessageId = null,

                IsForwarded = true,

                ForwardedFromUserId = originalMessage.SenderId,

                ForwardedFromUserName = originalMessage.Sender.UserName
            };

            return Result<PrivateMessageNotifierDto>.Success(dto);
        }
    }
}
