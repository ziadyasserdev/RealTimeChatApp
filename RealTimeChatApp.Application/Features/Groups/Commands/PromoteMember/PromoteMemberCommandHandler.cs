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

namespace RealTimeChatApp.Application.Features.Groups.Commands.PromoteMember
{
    public class PromoteMemberCommandHandler
     : IRequestHandler<PromoteMemberCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public PromoteMemberCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<string>> Handle(
            PromoteMemberCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");

            var ownerId = _currentUser.UserId!;

            var owner = await _unitOfWork.GroupMembers.Query()
                .FirstOrDefaultAsync(x =>
                    x.GroupId == request.GroupId &&
                    x.UserId == ownerId,
                    cancellationToken);

            if (owner is null || owner.Role != GroupRole.Owner.ToString())
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "Only owner can promote members.");

            var member = await _unitOfWork.GroupMembers.Query()
                .FirstOrDefaultAsync(x =>
                    x.GroupId == request.GroupId &&
                    x.UserId == request.UserId,
                    cancellationToken);

            if (member is null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Member not found.");

            if (member.Role == GroupRole.Admin.ToString())
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Member is already admin.");

            if (member.Role == GroupRole.Owner.ToString())
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Owner cannot be promoted.");

            member.Role = GroupRole.Admin.ToString();

            await _unitOfWork.SaveAsync();

            return Result<string>.Success("Member promoted successfully.");
        }
    }
}