using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.PrivateMessages.Dtos;
using RealTimeChatApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.PrivateMessages.Commands.DeletePrivateMessageForEveryone
{
    public class DeletePrivateMessageForEveryoneCommandHandler
    : IRequestHandler<
        DeletePrivateMessageForEveryoneCommand,
        Result<PrivateMessageNotifierDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public DeletePrivateMessageForEveryoneCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<PrivateMessageNotifierDto>> Handle(
            DeletePrivateMessageForEveryoneCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Result<PrivateMessageNotifierDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized.");
            }

            var userId = _currentUser.UserId!;

            var message = await _unitOfWork.PrivateMessages
                .Query()
                .Include(x => x.Sender)
                .FirstOrDefaultAsync(
                    x => x.Id == request.MessageId,
                    cancellationToken);

            if (message is null)
            {
                return Result<PrivateMessageNotifierDto>.Failure(
                    ResultStatus.NotFound,
                    "Message not found.");
            }

          
            if (message.SenderId != userId)
            {
                return Result<PrivateMessageNotifierDto>.Failure(
                    ResultStatus.Forbidden,
                    "Only the sender can delete this message for everyone.");
            }

            if (message.IsDeletedForEveryone)
            {
                return Result<PrivateMessageNotifierDto>.Failure(
                    ResultStatus.Conflict,
                    "Message already deleted.");
            }

            message.IsDeletedForEveryone = true;
            message.DeletedAt = DateTime.UtcNow;
            message.MessageType = MessageType.Deleted;
           

            await _unitOfWork.SaveAsync();

            return Result<PrivateMessageNotifierDto>.Success(
                new PrivateMessageNotifierDto
                {
                    MessageId = message.Id,
                    SenderId = message.SenderId,
                    SenderName = message.Sender.UserName!,
                    ReceiverId = message.ReceiverId,


                    Content = string.Empty,

                    MessageType = MessageType.Deleted,

                    ReplyToMessageId = message.ReplyToMessageId,

                   

                    SentAt = message.SentAt
                });
        }

    }
}