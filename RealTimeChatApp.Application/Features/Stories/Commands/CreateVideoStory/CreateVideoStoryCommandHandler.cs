using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Contracts.Storage;
using RealTimeChatApp.Application.Features.Stories.Commands.CreateImageStory;
using RealTimeChatApp.Application.Features.Stories.Dtos;
using RealTimeChatApp.Domain.Enums;
using RealTimeChatApp.Domain.Identity;
using RealTimeChatApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Commands.CreateVideoStory
{
    public class CreateVideoStoryCommandHandler
     : IRequestHandler<CreateVideoStoryCommand, Result<StoryNotifierDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileStorageService _fileStorage;

        public CreateVideoStoryCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            UserManager<ApplicationUser> userManager,
            IFileStorageService fileStorage)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _userManager = userManager;
            _fileStorage = fileStorage;
        }

        public async Task<Result<StoryNotifierDto>> Handle(
            CreateVideoStoryCommand request,
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

            var uploadResult = await _fileStorage.UploadVideoAsync(
                request.Video,
                CloudinaryFolders.Stories,
                cancellationToken);

            var story = new Story
            {
                UserId = user.Id,
                Type = StoryType.Video,
                MediaUrl = uploadResult.Url,
                MediaPublicId = uploadResult.PublicId,
                Caption = request.Caption,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };

            await _unitOfWork.Stories.AddAsync(story);

            await _unitOfWork.SaveAsync();

            return Result<StoryNotifierDto>.Success(new StoryNotifierDto
            {
                StoryId = story.Id,
                UserId = user.Id,
                UserName = user.DisplayName!,
                StoryType = StoryType.Video,
                MediaUrl = story.MediaUrl,
                MediaPublicId = story.MediaPublicId,
                Caption = story.Caption,
                CreatedAt = story.CreatedAt,
                ExpiresAt = story.ExpiresAt
            });
        }
    }
}
