using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.Stories.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Commands.RemoveStoryReaction
{
    public sealed class RemoveStoryReactionCommandHandler
    : IRequestHandler<RemoveStoryReactionCommand, Result<StoryReactionRemovedNotifierDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public RemoveStoryReactionCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<StoryReactionRemovedNotifierDto>> Handle(
            RemoveStoryReactionCommand request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Result<StoryReactionRemovedNotifierDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized.");
            }

            var story = await _unitOfWork.Stories.Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.StoryId,
                    cancellationToken);

            if (story is null)
            {
                return Result<StoryReactionRemovedNotifierDto>.Failure(
                    ResultStatus.NotFound,
                    "Story not found.");
            }

            var reaction = await _unitOfWork.StoryReactions.Query()
                .FirstOrDefaultAsync(
                    x => x.StoryId == request.StoryId &&
                         x.UserId == userId,
                    cancellationToken);

            if (reaction is null)
            {
                return Result<StoryReactionRemovedNotifierDto>.Failure(
                    ResultStatus.NotFound,
                    "Reaction not found.");
            }

            await using var transaction =
                await _unitOfWork.BeginTransactionAsync();

            try
            {
                _unitOfWork.StoryReactions.Delete(reaction);

                await _unitOfWork.SaveAsync();

                await transaction.CommitAsync();

                return Result<StoryReactionRemovedNotifierDto>.Success(
                    new StoryReactionRemovedNotifierDto
                    {
                        StoryId = story.Id,
                        UserId = userId
                    });
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
