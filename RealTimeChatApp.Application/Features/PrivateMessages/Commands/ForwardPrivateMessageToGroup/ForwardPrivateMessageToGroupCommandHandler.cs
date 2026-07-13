using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.GroupMessages.Dtos;
using RealTimeChatApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.PrivateMessages.Commands.ForwardPrivateMessageToGroup
{
    public class ForwardPrivateMessageToGroupCommandHandler
    : IRequestHandler<
        ForwardPrivateMessageToGroupCommand,
        Result<GroupMessageNotifierDto>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;


        public ForwardPrivateMessageToGroupCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }



        public async Task<Result<GroupMessageNotifierDto>> Handle(
            ForwardPrivateMessageToGroupCommand request,
            CancellationToken cancellationToken)
        {

            if (!_currentUser.IsAuthenticated)
            {
                return Result<GroupMessageNotifierDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized.");
            }


            var userId = _currentUser.UserId!;



            // check group
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



            // check membership
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



            // get original private message

            var originalMessage =
                await _unitOfWork.PrivateMessages.Query()

                .Include(x => x.Sender)

                .FirstOrDefaultAsync(
                    x => x.Id == request.MessageId,
                    cancellationToken);



            if (originalMessage is null)
            {
                return Result<GroupMessageNotifierDto>.Failure(
                    ResultStatus.NotFound,
                    "Message not found.");
            }




           
            var message = new GroupMessage
            {
                GroupId = request.GroupId,

                SenderId = userId,

                Content = originalMessage.Content,

                MessageType = originalMessage.MessageType,

                SentAt = DateTime.UtcNow,

                CreatedAt = DateTime.UtcNow,

                IsEdited = false,

                IsPinned = false,


                IsForwarded = true,

                ForwardedFromUserId = originalMessage.SenderId
            };



            await _unitOfWork.GroupMessages
                .AddAsync(message);


            await _unitOfWork.SaveAsync();




         

            var createdMessage =
                await _unitOfWork.GroupMessages.Query()

                .Include(x => x.Sender)

                .Include(x => x.ForwardedFromUser)

                .FirstAsync(
                    x => x.Id == message.Id,
                    cancellationToken);




            var dto = new GroupMessageNotifierDto
            {

                Id = createdMessage.Id,

                GroupId = createdMessage.GroupId,

                SenderId = createdMessage.SenderId,

                SenderName =
                    createdMessage.Sender.UserName!,


                Content = createdMessage.Content,


                MessageType =
                    createdMessage.MessageType.ToString(),


                SentAt = createdMessage.SentAt,


                IsEdited = false,


                IsPinned = false,


                IsForwarded = true,


                ForwardedFromUserId =
                    createdMessage.ForwardedFromUserId,


                ForwardedFromUserName =
                    createdMessage.ForwardedFromUser?.UserName
            };



            return Result<GroupMessageNotifierDto>
                .Success(dto);
        }
    }
}
