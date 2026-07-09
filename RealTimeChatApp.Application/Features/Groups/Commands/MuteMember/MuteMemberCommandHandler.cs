using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Commands.MuteMember
{
    public class MuteMemberCommandHandler
     : IRequestHandler<MuteMemberCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public MuteMemberCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<string>> Handle(
            MuteMemberCommand request,
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
                .Include(g => g.Members)
                .FirstOrDefaultAsync(
                    g => g.Id == request.GroupId,
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
                    "Only owner or admin can mute members.");
            }

            var targetMember = group.Members.FirstOrDefault(x =>
                x.UserId == request.UserId);

            if (targetMember is null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Member not found.");
            }

          
            if (targetMember.Role == GroupRole.Owner.ToString())
            {
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "The group owner cannot be muted.");
            }

           
            if (currentMember.Role == GroupRole.Admin.ToString() &&
                targetMember.Role == GroupRole.Admin.ToString())
            {
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "Admins cannot mute other admins.");
            }

            if (targetMember.IsMuted)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Member is already muted.");
            }

            targetMember.IsMuted = true;
            targetMember.MutedUntil = request.MutedUntil;

            await _unitOfWork.SaveAsync();

            return Result<string>.Success("Member muted successfully.");
        }
    }
}
