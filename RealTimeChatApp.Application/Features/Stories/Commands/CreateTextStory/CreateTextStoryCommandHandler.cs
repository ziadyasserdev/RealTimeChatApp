using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.Stories.Dtos;
using RealTimeChatApp.Domain.Enums;
using RealTimeChatApp.Domain.Identity;
using RealTimeChatApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Commands.CreateTextStory
{
    public class CreateTextStoryCommandHandler
     : IRequestHandler<CreateTextStoryCommand, Result<StoryNotifierDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateTextStoryCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _userManager = userManager;
        }

        public async Task<Result<StoryNotifierDto>> Handle(
            CreateTextStoryCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Result<StoryNotifierDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized.");
            }

            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.Id == _currentUser.UserId,
                    cancellationToken);

            if (user is null)
            {
                return Result<StoryNotifierDto>.Failure(
                    ResultStatus.NotFound,
                    "User not found.");
            }

            var story = new Story
            {
                UserId = user.Id,
                Type = StoryType.Text,
                Text = request.Content,
                BackgroundColor = request.BackgroundColor,
                TextColor = request.TextColor,
                CreatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddHours(24)
            };

            await _unitOfWork.Stories.AddAsync(story);

            await _unitOfWork.SaveAsync();

            var dto = new StoryNotifierDto
            {
                StoryId = story.Id,
                UserId = user.Id,
                UserName = user.DisplayName,
                StoryType = story.Type,
                Text = story.Text,
                BackgroundColor = story.BackgroundColor,
                TextColor = story.TextColor,
                CreatedAt = story.CreatedAt,
                ExpiresAt = story.ExpiresAt
            };

            return Result<StoryNotifierDto>.Success(dto);
        }
    }
}
