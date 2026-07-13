using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Commands.ViewStory
{
    public class ViewStoryCommandHandler
    : IRequestHandler<ViewStoryCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public ViewStoryCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<int>> Handle(
            ViewStoryCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Result<int>.Failure(
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
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    "Story not found.");
            }

            if (story.ExpiresAt <= DateTime.UtcNow)
            {
                return Result<int>.Failure(
                    ResultStatus.Failure,
                    "Story has expired.");
            }

       
            if (story.UserId == currentUserId)
            {
                return Result<int>.Failure(
                    ResultStatus.Failure,
                    "You cannot view your own story.");
            }

         
            var isBlocked = await _unitOfWork.UserBlocks.Query()
                .AnyAsync(x =>
                    (x.BlockerId == currentUserId &&
                     x.BlockedUserId == story.UserId)
                 ||
                    (x.BlockerId == story.UserId &&
                     x.BlockedUserId == currentUserId),
                    cancellationToken);

            if (isBlocked)
            {
                return Result<int>.Failure(
                    ResultStatus.Forbidden,
                    "You cannot view this story.");
            }

            var alreadyViewed = await _unitOfWork.StoryViews.Query()
                .AnyAsync(x =>
                    x.StoryId == request.StoryId &&
                    x.ViewerId == currentUserId,
                    cancellationToken);

            if (alreadyViewed)
            {
                return Result<int>.Success(request.StoryId);
            }

            var storyView = new StoryView
            {
                StoryId = request.StoryId,
                ViewerId = currentUserId,
                ViewedAt = DateTime.Now
                

            };

            await _unitOfWork.StoryViews.AddAsync(storyView);

            await _unitOfWork.SaveAsync();

            return Result<int>.Success(request.StoryId);
        }
    }
}
