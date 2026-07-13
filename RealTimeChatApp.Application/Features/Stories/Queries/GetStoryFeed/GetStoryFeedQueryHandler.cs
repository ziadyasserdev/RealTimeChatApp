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

namespace RealTimeChatApp.Application.Features.Stories.Queries.GetStoryFeed
{
    public class GetStoryFeedQueryHandler
      : IRequestHandler<GetStoryFeedQuery, Result<List<StoryUserFeedDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public GetStoryFeedQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<List<StoryUserFeedDto>>> Handle(
            GetStoryFeedQuery request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Result<List<StoryUserFeedDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized.");
            }

            var currentUserId = _currentUser.UserId!;

          
            var chatUsers = await _unitOfWork.PrivateMessages.Query()
                .Where(x =>
                    x.SenderId == currentUserId ||
                    x.ReceiverId == currentUserId)
                .Select(x =>
                    x.SenderId == currentUserId
                        ? x.ReceiverId
                        : x.SenderId)
                .Distinct()
                .ToListAsync(cancellationToken);
            var blockedUsers = await _unitOfWork.UserBlocks.Query()
    .Where(x =>
        x.BlockerId == currentUserId ||
        x.BlockedUserId == currentUserId)
    .Select(x =>
        x.BlockerId == currentUserId
            ? x.BlockedUserId
            : x.BlockerId)
    .ToListAsync(cancellationToken);
            chatUsers = chatUsers
    .Except(blockedUsers)
    .ToList();

            chatUsers.Add(currentUserId);

           
            var stories = await _unitOfWork.Stories.Query()
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.Views)
                .Where(x =>
                    chatUsers.Contains(x.UserId) &&
                    x.ExpiresAt > DateTime.UtcNow)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

            var feed = stories
                .GroupBy(x => x.UserId)
                .Select(group =>
                {
                    var storyDtos = group
                        .OrderBy(x => x.CreatedAt)
                        .Select(story => new StoryDto
                        {
                            StoryId = story.Id,
                            StoryType = story.Type,
                            MediaUrl = story.MediaUrl,
                            Caption = story.Caption,
                            Text = story.Text,
                            BackgroundColor = story.BackgroundColor,
                            TextColor = story.TextColor,
                            CreatedAt = story.CreatedAt,

                            IsViewed = story.Views.Any(v =>
                                v.ViewerId == currentUserId)
                        })
                        .ToList();

                    return new StoryUserFeedDto
                    {
                        UserId = group.Key,

                        UserName = group.First().User.UserName!,

                        ProfileImage = group.First().User.ProfilePictureUrl,

                        HasUnViewedStories = storyDtos.Any(x => !x.IsViewed),

                        Stories = storyDtos
                    };
                })
                .OrderByDescending(x => x.HasUnViewedStories)

              
                .ThenBy(x => x.UserId == currentUserId ? 0 : 1)

                .ThenBy(x => x.UserName)

                .ToList();

            return Result<List<StoryUserFeedDto>>
                .Success(feed);
        }
    }
}
