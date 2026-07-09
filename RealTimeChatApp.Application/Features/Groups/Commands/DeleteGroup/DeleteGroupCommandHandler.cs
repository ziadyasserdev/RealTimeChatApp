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

namespace RealTimeChatApp.Application.Features.Groups.Commands.DeleteGroup
{
    public class DeleteGroupCommandHandler
      : IRequestHandler<DeleteGroupCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public DeleteGroupCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<string>> Handle(
            DeleteGroupCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");

            var userId = _currentUser.UserId!;

            var group = await _unitOfWork.Groups
                .Query()
                .Include(x => x.Members)
                .FirstOrDefaultAsync(x =>
                    x.Id == request.GroupId,
                    cancellationToken);

            if (group is null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Group not found.");

            var owner = group.Members.FirstOrDefault(x =>
                x.UserId == userId &&
                x.Role == GroupRole.Owner.ToString());

            if (owner is null)
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "Only owner can delete this group.");

            await using var transaction =
                await _unitOfWork.BeginTransactionAsync();

            try
            {
                _unitOfWork.Groups.Delete(group);

                await _unitOfWork.SaveAsync();

                await transaction.CommitAsync(cancellationToken);

                return Result<string>.Success("Group deleted successfully.");
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}