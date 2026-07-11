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

namespace RealTimeChatApp.Application.Features.PrivateMessages.Commands.DeleteForMe
{
    public class DeletePrivateMessageForMeCommandHandler
     : IRequestHandler<
         DeletePrivateMessageForMeCommand,
         Result<DeletePrivateMessageNotifierDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public DeletePrivateMessageForMeCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<DeletePrivateMessageNotifierDto>> Handle(
            DeletePrivateMessageForMeCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Result<DeletePrivateMessageNotifierDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized.");
            }

            var userId = _currentUser.UserId!;

            var message = await _unitOfWork.PrivateMessages
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.MessageId,
                    cancellationToken);

            if (message is null)
            {
                return Result<DeletePrivateMessageNotifierDto>.Failure(
                    ResultStatus.NotFound,
                    "Message not found.");
            }

            if (message.SenderId != userId &&
                message.ReceiverId != userId)
            {
                return Result<DeletePrivateMessageNotifierDto>.Failure(
                    ResultStatus.Forbidden,
                    "You don't have permission to delete this message.");
            }

            if (message.SenderId == userId)
            {
                if (message.DeletedForSender)
                {
                    return Result<DeletePrivateMessageNotifierDto>.Failure(
                        ResultStatus.Conflict,
                        "Message already deleted for you.");
                }

                message.DeletedForSender = true;
            }

            if (message.ReceiverId == userId)
            {
                if (message.DeletedForReceiver)
                {
                    return Result<DeletePrivateMessageNotifierDto>.Failure(
                        ResultStatus.Conflict,
                        "Message already deleted for you.");
                }

                message.DeletedForReceiver = true;
            }

         

            _unitOfWork.PrivateMessages.Update(message);

            await _unitOfWork.SaveAsync();

            return Result<DeletePrivateMessageNotifierDto>.Success(
                new DeletePrivateMessageNotifierDto
                {
                    MessageId = message.Id,
                    UserId = userId
                });
        }
    }
}
