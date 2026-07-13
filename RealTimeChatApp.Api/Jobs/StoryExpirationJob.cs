using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Api.SignalR.NotifierService;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Contracts.Storage;
using RealTimeChatApp.Application.Features.Stories.Dtos;
using RealTimeChatApp.Domain.Enums;

namespace RealTimeChatApp.Api.Jobs
{
    public class StoryExpirationJob : IStoryExpirationJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorage;
        private readonly IChatHubNotifier _chatHubNotifier;
        private readonly ILogger<StoryExpirationJob> _logger;

        public StoryExpirationJob(
            IUnitOfWork unitOfWork,
            IFileStorageService fileStorage,
            IChatHubNotifier chatHubNotifier,
            ILogger<StoryExpirationJob> logger)
        {
            _unitOfWork = unitOfWork;
            _fileStorage = fileStorage;
            _chatHubNotifier = chatHubNotifier;
            _logger = logger;
        }

        public async Task ExecuteAsync(
            CancellationToken cancellationToken = default)
        {
            var expiredStories = await _unitOfWork.Stories.Query()
                .Where(x => x.ExpiresAt <= DateTime.UtcNow)
                .ToListAsync(cancellationToken);

            if (!expiredStories.Any())
                return;

            var deletedStories = new List<StoryDeletedNotifierDto>();

            await using var transaction =
                await _unitOfWork.BeginTransactionAsync();

            try
            {
                foreach (var story in expiredStories)
                {
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
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "Failed to delete story media from Cloudinary. StoryId: {StoryId}",
                            story.Id);
                    }

                    deletedStories.Add(new StoryDeletedNotifierDto
                    {
                        StoryId = story.Id,
                        UserId = story.UserId
                    });

                    _unitOfWork.Stories.Delete(story);
                }

                await _unitOfWork.SaveAsync();

                await transaction.CommitAsync();

                foreach (var story in deletedStories)
                {
                    try
                    {
                        await _chatHubNotifier.StoryDeletedAsync(story);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "Failed to notify clients about deleted story {StoryId}",
                            story.StoryId);
                    }
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                _logger.LogError(
                    ex,
                    "An error occurred while deleting expired stories.");

                throw;
            }
        }
    }
}

