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

namespace RealTimeChatApp.Application.Features.PrivateMessages.Commands.EditMessage
{
    public class EditPrivateMessageCommandHandler
     : IRequestHandler<
         EditPrivateMessageCommand,
         Result<PrivateMessageNotifierDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public EditPrivateMessageCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<PrivateMessageNotifierDto>> Handle(
            EditPrivateMessageCommand request,
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
                    "You can edit only your own messages.");
            }

            message.Content = request.Content;
            message.IsEdited = true;
            message.EditedAt = DateTime.Now;
           
            _unitOfWork.PrivateMessages.Update(message);

            await _unitOfWork.SaveAsync();

            return Result<PrivateMessageNotifierDto>.Success(
                new PrivateMessageNotifierDto
                {
                    MessageId = message.Id,
                    SenderId = message.SenderId,
                    SenderName = message.Sender.UserName!,
                    ReceiverId = message.ReceiverId,
                    Content = message.Content,
                    MessageType = message.MessageType,
                    ReplyToMessageId = message.ReplyToMessageId,
                    SentAt = message.SentAt,
                    IsEdited = true
                });
        }
    }
}
