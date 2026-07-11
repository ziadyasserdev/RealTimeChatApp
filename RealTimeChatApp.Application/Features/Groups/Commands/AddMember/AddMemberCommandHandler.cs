using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Domain.Enums;
using RealTimeChatApp.Domain.Identity;
using RealTimeChatApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Commands.AddMember
{
    public class AddMemberCommandHandler
     : IRequestHandler<AddMemberCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<ApplicationUser> _userManager;

        public AddMemberCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _userManager = userManager;
        }

        public async Task<Result<string>> Handle(
            AddMemberCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            var currentUserId = _currentUser.UserId!;

            var group = await _unitOfWork.Groups
                .Query()
                .Include(x => x.Members)
                .FirstOrDefaultAsync(
                    x => x.Id == request.GroupId,
                    cancellationToken);

            if (group is null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Group not found.");
            }

            var currentMember = group.Members.FirstOrDefault(x =>
                x.UserId == currentUserId);

            if (currentMember is null)
            {
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "You are not a member of this group.");
            }

            if (currentMember.Role != GroupRole.Owner.ToString() &&
                currentMember.Role != GroupRole.Admin.ToString())
            {
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "Only owner or admin can add members.");
            }

            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user is null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "User not found.");
            }

            var alreadyMember = group.Members.Any(x =>
                x.UserId == request.UserId);

            if (alreadyMember)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "User is already a member.");
            }

            if (group.Members.Count >= group.MaxMembers)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Group has reached maximum members.");
            }

            var member = new GroupMember
            {
                GroupId = group.Id,
                UserId = request.UserId,
                Role = GroupRole.Member.ToString(),
                Nickname = user.UserName,
                JoinedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                IsMuted = false,
                MutedUntil = null,
              
            };

            await _unitOfWork.GroupMembers.AddAsync(member);

            await _unitOfWork.SaveAsync();

            return Result<string>.Success("Member added successfully.");
        }
    }
}