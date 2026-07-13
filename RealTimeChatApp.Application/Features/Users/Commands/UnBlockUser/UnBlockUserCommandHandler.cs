using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Users.Commands.UnBlockUser
{
    public class UnBlockUserCommandHandler
    : IRequestHandler<UnBlockUserCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public UnBlockUserCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<string>> Handle(
            UnBlockUserCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized.");
            }

            var currentUserId = _currentUser.UserId!;

            var block = await _unitOfWork.UserBlocks
                .Query()
                .FirstOrDefaultAsync(x =>
                    x.BlockerId == currentUserId &&
                    x.BlockedUserId == request.UserId,
                    cancellationToken);

            if (block is null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "User is not blocked.");
            }

            _unitOfWork.UserBlocks.Delete(block);

            await _unitOfWork.SaveAsync();

            return Result<string>.Success("User unblocked successfully.");
        }
    }
}
