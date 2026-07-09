using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Domain.Enums;
using RealTimeChatApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Commands.SendGroupMessage
{
    public class SendGroupMessageCommandHandler : IRequestHandler<SendGroupMessageCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
      
        public SendGroupMessageCommandHandler(
            IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(
    SendGroupMessageCommand request,
    CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<int>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized");

            var userId = currentUserService.UserId!;

            var member = await unitOfWork.GroupMembers.Query()

                .FirstOrDefaultAsync(x =>
                    x.GroupId == request.GroupId &&
                    x.UserId == userId,
                    cancellationToken);

            if (member is null)
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    "You are not a member.");
            var group = await unitOfWork.Groups.GetByIdAsync(request.GroupId);
            if(group is null)
            {
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    "Group not found.");
            }

            if (group.OnlyAdminsCanSendMessages &&
    member.Role != GroupRole.Owner.ToString() &&
    member.Role != GroupRole.Admin.ToString())
            {
                return Result<int>.Failure(
                    ResultStatus.Forbidden,
                    "Only admins can send messages while announcement mode is enabled.");
            }

            if (member.IsMuted &&
                member.MutedUntil > DateTime.UtcNow)
            {
                return Result<int>.Failure(
                    ResultStatus.Forbidden,
                    "You are muted.");
            }

            var message = new GroupMessage
            {
                GroupId = request.GroupId,
                SenderId = userId,
                Content = request.Content,
                MessageType = "Text",
                SentAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                IsPinned = false
            };

            await unitOfWork.GroupMessages.AddAsync(message);

            await unitOfWork.SaveAsync();

            return Result<int>.Success(message.Id);
        }
    }
}
