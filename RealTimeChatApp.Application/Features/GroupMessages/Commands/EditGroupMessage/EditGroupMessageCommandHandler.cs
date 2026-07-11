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

namespace RealTimeChatApp.Application.Features.GroupMessages.Commands.EditGroupMessage
{
    public class EditGroupMessageCommandHandler
       : IRequestHandler<EditGroupMessageCommand, Result<EditGroupMessageNotifierDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public EditGroupMessageCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<EditGroupMessageNotifierDto>> Handle(
            EditGroupMessageCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Result<EditGroupMessageNotifierDto>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            var userId = _currentUser.UserId!;

            var message = await _unitOfWork.GroupMessages
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.MessageId,
                    cancellationToken);

            if (message is null)
            {
                return Result<EditGroupMessageNotifierDto>.Failure(
                    ResultStatus.NotFound,
                    "Message not found.");
            }

            if (message.SenderId != userId)
            {
                return Result<EditGroupMessageNotifierDto>.Failure(
                    ResultStatus.Forbidden,
                    "You can edit only your own messages.");
            }

            if (message.Content == request.Content.Trim())
            {
                return Result<EditGroupMessageNotifierDto>.Failure(
                    ResultStatus.Conflict,
                    "No changes detected.");
            }

            message.Content = request.Content.Trim();
            message.IsEdited = true;
            message.EditedAt = DateTime.Now;
            message.UpdatedAt = DateTime.Now;

            await _unitOfWork.SaveAsync();
            var dto = new EditGroupMessageNotifierDto
            {
                GroupId = message.GroupId,
                MessageId = message.Id,
                Content = message.Content,
                IsEdited = message.IsEdited,
                EditedAt = message.EditedAt
            };

            return Result<EditGroupMessageNotifierDto>.Success(dto);
        }
    }
}
