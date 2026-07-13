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

namespace RealTimeChatApp.Application.Features.Stories.Queries.GetStoryViewers
{
    public class GetStoryViewersQueryHandler
        : IRequestHandler<GetStoryViewersQuery, Result<StoryViewersDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public GetStoryViewersQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<StoryViewersDto>> Handle(
            GetStoryViewersQuery request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Result<StoryViewersDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized.");
            }

            var currentUserId = _currentUser.UserId!;

            var story = await _unitOfWork.Stories.Query()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.StoryId,
                    cancellationToken);

            if (story is null)
            {
                return Result<StoryViewersDto>.Failure(
                    ResultStatus.NotFound,
                    "Story not found.");
            }

            if (story.UserId != currentUserId)
            {
                return Result<StoryViewersDto>.Failure(
                    ResultStatus.Forbidden,
                    "You are not allowed to view this story viewers.");
            }

            var viewers = await _unitOfWork.StoryViews.Query()
                .AsNoTracking()
                .Where(x => x.StoryId == request.StoryId)
                .Include(x => x.Viewer)
                .OrderByDescending(x => x.ViewedAt)
                .Select(x => new StoryViewerDto
                {
                    ViewerId = x.ViewerId,
                    UserName = x.Viewer.UserName!,
                    ProfilePictureUrl = x.Viewer.ProfilePictureUrl,
                    ViewedAt = x.ViewedAt
                })
                .ToListAsync(cancellationToken);

            var response = new StoryViewersDto
            {
                ViewsCount = viewers.Count,
                Viewers = viewers
            };

            return Result<StoryViewersDto>.Success(response);
        }
    }
}

