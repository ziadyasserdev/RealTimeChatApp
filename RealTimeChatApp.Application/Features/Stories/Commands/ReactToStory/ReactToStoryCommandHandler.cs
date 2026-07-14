using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.Stories.Dtos;
using RealTimeChatApp.Domain.Identity;
using RealTimeChatApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Commands.ReactToStory
{

    public sealed class ReactToStoryCommandHandler
      : IRequestHandler<ReactToStoryCommand, Result<StoryReactionNotifierDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReactToStoryCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _userManager = userManager;
        }

        public async Task<Result<StoryReactionNotifierDto>> Handle(
            ReactToStoryCommand request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Result<StoryReactionNotifierDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized.");
            }

            var story = await _unitOfWork.Stories.Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.StoryId,
                    cancellationToken);

            if (story is null)
            {
                return Result<StoryReactionNotifierDto>.Failure(
                    ResultStatus.NotFound,
                    "Story not found.");
            }

            if (story.ExpiresAt <= DateTime.UtcNow)
            {
                return Result<StoryReactionNotifierDto>.Failure(
                    ResultStatus.Failure,
                    "Story has expired.");
            }

            var reaction = await _unitOfWork.StoryReactions.Query()
                .FirstOrDefaultAsync(
                    x => x.StoryId == request.StoryId &&
                         x.UserId == userId,
                    cancellationToken);

            bool isNewReaction = false;

            await using var transaction =
                await _unitOfWork.BeginTransactionAsync();

            try
            {
                if (reaction is null)
                {
                    reaction = new StoryReaction
                    {
                        StoryId = request.StoryId,
                        UserId = userId,
                        ReactionType = request.Type,
                        ReactedAt = DateTime.UtcNow
                    };

                    await _unitOfWork.StoryReactions.AddAsync(reaction);

                    isNewReaction = true;
                }
                else
                {
                    if (reaction.ReactionType == request.Type)
                    {
                        return Result<StoryReactionNotifierDto>.Failure(
                            ResultStatus.Conflict,
                            "You already reacted with this reaction.");
                    }

                    reaction.ReactionType = request.Type;
                    reaction.ReactedAt = DateTime.UtcNow;

                    _unitOfWork.StoryReactions.Update(reaction);
                }

                await _unitOfWork.SaveAsync();

                await transaction.CommitAsync();

                var user = await _userManager.FindByIdAsync(userId);

                if (user is null)
                {
                    return Result<StoryReactionNotifierDto>.Failure(
                        ResultStatus.NotFound,
                        "User not found.");
                }

                var dto = new StoryReactionNotifierDto
                {
                    StoryId = story.Id,
                    UserId = user.Id,
                    UserName = user.UserName!,
                    ProfilePictureUrl = user.ProfilePictureUrl,
                    Type = reaction.ReactionType,
                    ReactedAt = reaction.ReactedAt,
                    IsNewReaction = isNewReaction
                };

                return Result<StoryReactionNotifierDto>.Success(dto);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}

