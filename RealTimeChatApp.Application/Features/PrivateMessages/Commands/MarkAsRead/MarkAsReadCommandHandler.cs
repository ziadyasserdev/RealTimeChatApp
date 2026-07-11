using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.PrivateMessages.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.PrivateMessages.Commands.MarkAsRead
{

    public class MarkPrivateMessageAsReadCommandHandler
    : IRequestHandler<
        MarkPrivateMessageAsReadCommand,
        Result<PrivateMessageNotifierDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public MarkPrivateMessageAsReadCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<PrivateMessageNotifierDto>> Handle(
            MarkPrivateMessageAsReadCommand request,
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

            if (message.ReceiverId != userId)
            {
                return Result<PrivateMessageNotifierDto>.Failure(
                    ResultStatus.Forbidden,
                    "Only receiver can mark message as read.");
            }

            if (message.IsRead)
            {
                return Result<PrivateMessageNotifierDto>.Success(
                    new PrivateMessageNotifierDto
                    {
                        MessageId = message.Id,
                        SenderId = message.SenderId,
                        ReceiverId = message.ReceiverId,
                        IsRead = true,
                        ReadAt = message.ReadAt
                    });
            }

            message.IsRead = true;
            message.ReadAt = DateTime.Now;
        

            await _unitOfWork.SaveAsync();

            return Result<PrivateMessageNotifierDto>.Success(
                new PrivateMessageNotifierDto
                {
                    MessageId = message.Id,
                    SenderId = message.SenderId,
                    ReceiverId = message.ReceiverId,
                    IsRead = true,
                    ReadAt = message.ReadAt
                });
        }
    }
}