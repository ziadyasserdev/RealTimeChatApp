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

namespace RealTimeChatApp.Application.Features.Groups.Commands.RemoveMember
{
    public class RemoveMemberCommandHandler
      : IRequestHandler<RemoveMemberCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public RemoveMemberCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<string>> Handle(
            RemoveMemberCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            var currentUserId = _currentUser.UserId!;

         
            var currentMember = await _unitOfWork.GroupMembers
                .Query()
                .FirstOrDefaultAsync(x =>
                    x.GroupId == request.GroupId &&
                    x.UserId == currentUserId,
                    cancellationToken);

            if (currentMember is null)
            {
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "You are not a member of this group.");
            }

         
            var targetMember = await _unitOfWork.GroupMembers
                .Query()
                .FirstOrDefaultAsync(x =>
                    x.GroupId == request.GroupId &&
                    x.UserId == request.UserId,
                    cancellationToken);

            if (targetMember is null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Member not found.");
            }

          
            if (targetMember.Role == GroupRole.Owner.ToString())
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Owner cannot be removed.");
            }

           
            if (currentMember.Role == GroupRole.Member.ToString())
            {
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "You don't have permission to remove members.");
            }

         
            if (currentMember.Role == GroupRole.Admin.ToString() &&
                targetMember.Role == GroupRole.Admin.ToString())
            {
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "Admin cannot remove another admin.");
            }

            await using var transaction =
                await _unitOfWork.BeginTransactionAsync();

            try
            {
                _unitOfWork.GroupMembers.Delete(targetMember);

                await _unitOfWork.SaveAsync();

                await transaction.CommitAsync(cancellationToken);

                return Result<string>.Success(
                    "Member removed successfully.");
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);

                throw;
            }
        }
    }
}
