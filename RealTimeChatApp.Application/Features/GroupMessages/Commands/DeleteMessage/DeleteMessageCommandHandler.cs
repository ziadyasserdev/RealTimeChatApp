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

namespace RealTimeChatApp.Application.Features.GroupMessages.Commands.DeleteMessage
{
    public class DeleteGroupMessageCommandHandler
     : IRequestHandler<
         DeleteGroupMessageCommand,
         Result<MessageDeletedNotifierDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public DeleteGroupMessageCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<MessageDeletedNotifierDto>> Handle(
            DeleteGroupMessageCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Result<MessageDeletedNotifierDto>.Failure(
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
                return Result<MessageDeletedNotifierDto>.Failure(
                    ResultStatus.NotFound,
                    "Message not found.");
            }

            if (message.SenderId != userId)
            {
                return Result<MessageDeletedNotifierDto>.Failure(
                    ResultStatus.Forbidden,
                    "You can delete only your own messages.");
            }

            _unitOfWork.GroupMessages.Delete(message);

            await _unitOfWork.SaveAsync();

            return Result<MessageDeletedNotifierDto>.Success(
                new MessageDeletedNotifierDto
                {
                    GroupId = message.GroupId,
                    MessageId = message.Id
                });
        }
    }
}