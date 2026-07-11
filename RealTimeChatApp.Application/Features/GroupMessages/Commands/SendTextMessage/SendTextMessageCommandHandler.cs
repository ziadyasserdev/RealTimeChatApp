using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.GroupMessages.Dtos;
using RealTimeChatApp.Application.Features.Groups.Dtos;
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
                    "Unauthorized.");
            }


            var userId = _currentUser.UserId!;



            var member = await _unitOfWork.GroupMembers.Query()
                .FirstOrDefaultAsync(
                    x =>
                    x.GroupId == request.GroupId &&
                    x.UserId == userId,
                    cancellationToken);


            if (member is null)
            {
                return Result<GroupMessageNotifierDto>.Failure(
                    ResultStatus.Forbidden,
                    "You are not a member of this group.");
            }




            var group = await _unitOfWork.Groups.Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.GroupId,
                    cancellationToken);



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
                (member.MutedUntil == null ||
                 member.MutedUntil > DateTime.UtcNow))
            {
                return Result<GroupMessageNotifierDto>.Failure(
                    ResultStatus.Forbidden,
                    "You are muted.");
            }





            if (request.ReplyToMessageId.HasValue)
            {

                var exists = await _unitOfWork.GroupMessages.Query()
                    .AnyAsync(
                        x =>
                        x.Id == request.ReplyToMessageId &&
                        x.GroupId == request.GroupId,
                        cancellationToken);


                if (!exists)
                {
                    return Result<GroupMessageNotifierDto>.Failure(
                        ResultStatus.NotFound,
                        "Reply message not found.");
                }
            }




            var message = new GroupMessage
            {
                GroupId = request.GroupId,

                SenderId = userId,

                Content = request.Content,

                MessageType = MessageType.Text,

                ReplyToMessageId = request.ReplyToMessageId,

                SentAt = DateTime.UtcNow,

                CreatedAt = DateTime.UtcNow,

                IsPinned = false,

                IsEdited = false
            };



            await _unitOfWork.GroupMessages.AddAsync(message);


            await _unitOfWork.SaveAsync();



          
            var createdMessage = await _unitOfWork.GroupMessages.Query()

                .Include(x => x.Sender)

                .Include(x => x.ReplyToMessage)
                    .ThenInclude(x => x!.Sender)

                .FirstAsync(
                    x => x.Id == message.Id,
                    cancellationToken);



            var dto = new GroupMessageNotifierDto
            {
                Id = createdMessage.Id,

                GroupId = createdMessage.GroupId,

                SenderId = createdMessage.SenderId,

                SenderName = createdMessage.Sender.UserName!,

                Content = createdMessage.Content,

                MessageType = createdMessage.MessageType.ToString(),

                SentAt = createdMessage.SentAt,

                IsEdited = createdMessage.IsEdited,

                IsPinned = createdMessage.IsPinned,
               

                ReplyToMessageId = createdMessage.ReplyToMessageId,

                ReplyContent = createdMessage.ReplyToMessage?.Content,

                ReplySenderName =
                    createdMessage.ReplyToMessage?.Sender.UserName
            };



            return Result<GroupMessageNotifierDto>.Success(dto);

        }
    }
}
