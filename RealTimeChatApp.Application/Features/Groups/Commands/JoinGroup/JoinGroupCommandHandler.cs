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

namespace RealTimeChatApp.Application.Features.Groups.Commands.JoinGroup
{
    public class JoinGroupCommandHandler
      : IRequestHandler<JoinGroupCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public JoinGroupCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            JoinGroupCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            var userId = currentUserService.UserId!;

            // Check Group Exists
            var group = await unitOfWork.Groups.Query()
                .FirstOrDefaultAsync(x => x.Id == request.GroupId, cancellationToken);

            if (group is null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Group not found.");
            }

            // Already Member
            var isMember = await unitOfWork.GroupMembers.Query()
                .AnyAsync(x =>
                    x.GroupId == request.GroupId &&
                    x.UserId == userId,
                    cancellationToken);

            if (isMember)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "You are already a member of this group.");
            }

           
            var membersCount = await unitOfWork.GroupMembers.Query()
                .CountAsync(x => x.GroupId == request.GroupId, cancellationToken);

            if (membersCount >= group.MaxMembers)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "This group has reached the maximum number of members.");
            }

            
            if (group.IsPrivate)
            {
                if (string.IsNullOrWhiteSpace(request.InviteCode))
                {
                    return Result<string>.Failure(
                        ResultStatus.Failure,
                        "Invite code is required.");
                }

                if (!string.Equals(group.InviteCode, request.InviteCode))
                {
                    return Result<string>.Failure(
                        ResultStatus.Failure,
                        "Invalid invite code.");
                }
            }

            var member = new GroupMember
            {
                GroupId = group.Id,
                UserId = userId,
                Nickname = currentUserService.UserName!,
                Role = GroupRole.Member.ToString(),
                JoinedAt = DateTime.Now,
                CreatedAt = DateTime.Now
            };

            await unitOfWork.GroupMembers.AddAsync(member);

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Joined successfully.");
        }
    }
}
