using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Contracts.Storage;
using RealTimeChatApp.Application.Features.Stories.Dtos;
using RealTimeChatApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Commands.DeleteStory
{
    public class DeleteStoryCommandHandler
     : IRequestHandler<DeleteStoryCommand, Result<StoryDeletedNotifierDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly IFileStorageService _fileStorage;

        public DeleteStoryCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            IFileStorageService fileStorage)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _fileStorage = fileStorage;
        }

        public async Task<Result<StoryDeletedNotifierDto>> Handle(
            DeleteStoryCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Result<StoryDeletedNotifierDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized.");
            }

            var currentUserId = _currentUser.UserId!;

            var story = await _unitOfWork.Stories.Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.StoryId,
                    cancellationToken);

            if (story is null)
            {
                return Result<StoryDeletedNotifierDto>.Failure(
                    ResultStatus.NotFound,
                    "Story not found.");
            }

            if (story.UserId != currentUserId)
            {
                return Result<StoryDeletedNotifierDto>.Failure(
                    ResultStatus.Forbidden,
                    "You are not allowed to delete this story.");
            }

            await using var transaction =
                await _unitOfWork.BeginTransactionAsync();

            try
            {
                if (story.Type == StoryType.Image &&
      !string.IsNullOrWhiteSpace(story.MediaPublicId))
                {
                    await _fileStorage.DeleteImageAsync(
                        story.MediaPublicId,
                        cancellationToken);
                }
                else if (story.Type == StoryType.Video &&
                         !string.IsNullOrWhiteSpace(story.MediaPublicId))
                {
                    await _fileStorage.DeleteVideoAsync(
                        story.MediaPublicId,
                        cancellationToken);
                }

                _unitOfWork.Stories.Delete(story);

                await _unitOfWork.SaveAsync();

                await transaction.CommitAsync();

                return Result<StoryDeletedNotifierDto>.Success(
                    new StoryDeletedNotifierDto
                    {
                        StoryId = story.Id,
                        UserId = story.UserId
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
