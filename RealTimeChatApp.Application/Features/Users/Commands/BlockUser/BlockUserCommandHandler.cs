using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Domain.Identity;
using RealTimeChatApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Users.Commands.BlockUser
{
    public class BlockUserCommandHandler
      : IRequestHandler<BlockUserCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlockUserCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _userManager = userManager;
        }

        public async Task<Result<string>> Handle(
            BlockUserCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized.");
            }

            var currentUserId = _currentUser.UserId!;

            if (currentUserId == request.UserId)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "You cannot block yourself.");
            }

            var userExists = await _userManager.Users
                .AnyAsync(x => x.Id == request.UserId, cancellationToken);

            if (!userExists)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "User not found.");
            }

            var alreadyBlocked = await _unitOfWork.UserBlocks
                .Query()
                .AnyAsync(x =>
                    x.BlockerId == currentUserId &&
                    x.BlockedUserId == request.UserId,
                    cancellationToken);

            if (alreadyBlocked)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "User already blocked.");
            }

            var block = new UserBlock
            {
                BlockerId = currentUserId,
                BlockedUserId = request.UserId,
                BlockedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.UserBlocks.AddAsync(block);

            await _unitOfWork.SaveAsync();

            return Result<string>.Success("User blocked successfully.");
        }
    }
    }
