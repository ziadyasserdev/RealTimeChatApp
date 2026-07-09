using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Domain.Enums;
using RealTimeChatApp.Domain.Helpers;
using RealTimeChatApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Commands.CreateGroup
{
    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public CreateGroupCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(
   CreateGroupCommand request,
   CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<int>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated");

            var userId = currentUserService.UserId!;

            var groupName = request.Name.Trim();

            var isExist = await unitOfWork.Groups.Query()
                .AnyAsync(x =>
                    x.CreatedById == userId &&
                    x.Name.ToLower() == groupName.ToLower(),
                    cancellationToken);

            if (isExist)
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "You already have a group with the same name.");

            await using var transaction =
                await unitOfWork.BeginTransactionAsync();

            try
            {
                var group = new Group
                {
                    Name = groupName,
                    Description = request.Description?.Trim(),

                    MaxMembers = request.MaxMembers,
                    CreatedById = userId,
                    IsPrivate = request.IsPrivate,
                    CreatedAt = DateTime.UtcNow
                };

                if (request.IsPrivate)
                {
                    string inviteCode;

                    do
                    {
                        inviteCode = InviteCodeGenerator.Generate();
                    }
                    while (await unitOfWork.Groups.Query()
                        .AnyAsync(x => x.InviteCode == inviteCode,
                            cancellationToken));

                    group.InviteCode = inviteCode;
                }

                await unitOfWork.Groups.AddAsync(group);

                await unitOfWork.SaveAsync();

                var owner = new GroupMember
                {
                    GroupId = group.Id,
                    UserId = userId,
                    Nickname = currentUserService.UserName!,
                    Role = GroupRole.Owner.ToString(),
                    JoinedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    IsMuted = false,

                };

                await unitOfWork.GroupMembers.AddAsync(owner);

                await unitOfWork.SaveAsync();

                await transaction.CommitAsync(cancellationToken);

                return Result<int>.Success(group.Id);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);

                return Result<int>.Failure(
                    ResultStatus.Failure,
                    "An error occurred while creating the group.");
            }
        }

    }
}
