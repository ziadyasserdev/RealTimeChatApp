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

namespace RealTimeChatApp.Application.Features.Groups.Commands.LeaveGroup
{
    public class LeaveGroupCommandHandler
     : IRequestHandler<LeaveGroupCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public LeaveGroupCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            LeaveGroupCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUserService.IsAuthenticated)
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");

            var userId = _currentUserService.UserId!;

            var member = await _unitOfWork.GroupMembers.Query()
                .FirstOrDefaultAsync(x =>
                    x.GroupId == request.GroupId &&
                    x.UserId == userId,
                    cancellationToken);

            if (member is null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "You are not a member of this group.");


            if (member.Role == GroupRole.Owner.ToString())
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Group owner cannot leave the group. Transfer ownership or delete the group.");

            _unitOfWork.GroupMembers.Delete(member);

            await _unitOfWork.SaveAsync();

            return Result<string>.Success("You left the group successfully.");
        }
    }
    }
