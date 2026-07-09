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

namespace RealTimeChatApp.Application.Features.Groups.Commands.ChangeAnnouncementMode
{
    public class ChangeAnnouncementModeCommandHandler
        : IRequestHandler<ChangeAnnouncementModeCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public ChangeAnnouncementModeCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<string>> Handle(
            ChangeAnnouncementModeCommand request,
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

            var currentMember = group.Members
                .FirstOrDefault(x => x.UserId == currentUserId);

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
                    "Only owner or admin can change announcement mode.");
            }

            if (group.OnlyAdminsCanSendMessages == request.Enable)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    request.Enable
                        ? "Announcement mode is already enabled."
                        : "Announcement mode is already disabled.");
            }

            group.OnlyAdminsCanSendMessages = request.Enable;

            await _unitOfWork.SaveAsync();

            return Result<string>.Success(
                request.Enable
                    ? "Announcement mode enabled successfully."
                    : "Announcement mode disabled successfully.");
        }
    }
}
