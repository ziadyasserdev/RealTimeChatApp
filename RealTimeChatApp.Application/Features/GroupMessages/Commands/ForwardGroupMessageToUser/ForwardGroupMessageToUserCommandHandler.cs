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

namespace RealTimeChatApp.Application.Features.GroupMessages.Commands.ForwardGroupMessageToUser
{
    public class ForwardGroupMessageToUserCommandHandler
    : IRequestHandler<
        ForwardGroupMessageToUserCommand,
        Result<PrivateMessageNotifierDto>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<ApplicationUser> _userManager;


        public ForwardGroupMessageToUserCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _userManager = userManager;
        }


        public async Task<Result<PrivateMessageNotifierDto>> Handle(
            ForwardGroupMessageToUserCommand request,
            CancellationToken cancellationToken)
        {

            if (!_currentUser.IsAuthenticated)
            {
                return Result<PrivateMessageNotifierDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized.");
            }


            var userId = _currentUser.UserId!;



            var originalMessage =
                await _unitOfWork.GroupMessages.Query()

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



            var isMember =
                await _unitOfWork.GroupMembers.Query()
                .AnyAsync(
                    x =>
                    x.GroupId == originalMessage.GroupId &&
                    x.UserId == userId,
                    cancellationToken);



            if (!isMember)
            {
                return Result<PrivateMessageNotifierDto>.Failure(
                    ResultStatus.Forbidden,
                    "You are not a member of this group.");
            }



            var receiver =
                await _userManager.Users
                .FirstOrDefaultAsync(
                    x => x.Id == request.ReceiverId,
                    cancellationToken);



            if (receiver is null)
            {
                return Result<PrivateMessageNotifierDto>.Failure(
                    ResultStatus.NotFound,
                    "Receiver not found.");
            }




            var message = new PrivateMessage
            {
                SenderId = userId,

                ReceiverId = request.ReceiverId,

                Content = originalMessage.Content,

                MessageType = originalMessage.MessageType,

                SentAt = DateTime.UtcNow,

                CreatedAt = DateTime.UtcNow,

                IsEdited = false,

                IsRead = false,

                IsForwarded = true,

                ForwardedFromUserId = originalMessage.SenderId,

                DeletedForSender = false,

                DeletedForReceiver = false,

                IsDeletedForEveryone = false
            };



            await _unitOfWork.PrivateMessages.AddAsync(message);

            await _unitOfWork.SaveAsync();



            var dto = new PrivateMessageNotifierDto
            {
                MessageId = message.Id,

                SenderId = userId,

                SenderName = _currentUser.UserName!,

                ReceiverId = request.ReceiverId,

                Content = message.Content,

                MessageType = message.MessageType,

                SentAt = message.SentAt,

                IsEdited = false,

                IsRead = false,

                ReadAt = null,

                ReplyToMessageId = null,

                IsForwarded = true,

                ForwardedFromUserId = originalMessage.SenderId,

                ForwardedFromUserName =
                    originalMessage.Sender.UserName
            };


            return Result<PrivateMessageNotifierDto>
                .Success(dto);
        }
    }
}
