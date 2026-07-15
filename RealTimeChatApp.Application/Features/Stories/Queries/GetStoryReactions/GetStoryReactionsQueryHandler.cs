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

namespace RealTimeChatApp.Application.Features.Stories.Queries.GetStoryReactions
{
    public  class GetStoryReactionsQueryHandler
       : IRequestHandler<GetStoryReactionsQuery, Result<StoryReactionsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetStoryReactionsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<StoryReactionsDto>> Handle(
            GetStoryReactionsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Result<StoryReactionsDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized.");
            }

            var story = await _unitOfWork.Stories.Query()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.StoryId,
                    cancellationToken);

            if (story is null)
            {
                return Result<StoryReactionsDto>.Failure(
                    ResultStatus.NotFound,
                    "Story not found.");
            }

         
            if (story.UserId != userId)
            {
                return Result<StoryReactionsDto>.Failure(
                    ResultStatus.Forbidden,
                    "You are not allowed to view story reactions.");
            }

            var reactions = await _unitOfWork.StoryReactions.Query()
                .AsNoTracking()
                .Where(x => x.StoryId == request.StoryId)
                .OrderByDescending(x => x.ReactedAt)
                .Select(x => new StoryReactionDto
                {
                    UserId = x.UserId,
                    UserName = x.User.UserName!,
                    ProfilePictureUrl = x.User.ProfilePictureUrl,
                    Type = x.ReactionType,
                    ReactedAt = x.ReactedAt
                })
                .ToListAsync(cancellationToken);

            var dto = new StoryReactionsDto
            {
                Count = reactions.Count,
                Reactions = reactions
            };

            return Result<StoryReactionsDto>.Success(dto);
        }
    }
}
