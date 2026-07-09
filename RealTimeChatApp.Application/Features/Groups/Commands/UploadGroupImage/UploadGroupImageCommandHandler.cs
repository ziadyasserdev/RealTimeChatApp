using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Contracts.Services;
using RealTimeChatApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Commands.UploadGroupImage
{
    public class UploadGroupImageCommandHandler
       : IRequestHandler<UploadGroupImageCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IFileService fileService;

        public UploadGroupImageCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.fileService = fileService;
        }

        public async Task<Result<string>> Handle(
            UploadGroupImageCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");

            var userId = currentUserService.UserId!;

            var group = await unitOfWork.Groups
                .Query()
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);

            if (group is null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Group not found.");

            var member = group.Members
                .FirstOrDefault(x => x.UserId == userId);

            if (member is null)
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "You are not a member of this group.");

            if (member.Role != GroupRole.Owner.ToString() &&
                member.Role != GroupRole.Admin.ToString())
            {
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "Only owner or admin can upload the group image.");
            }

           
            if (!string.IsNullOrWhiteSpace(group.ImageUrl))
            {
                fileService.Remove(group.ImageUrl);
            }

            var uploadResult = await fileService.UploadImageAsync(request.Image);

            if (!uploadResult.IsSuccess)
            {
                return Result<string>.Failure(
                    uploadResult.Status,
                    uploadResult.Error);
            }

            group.ImageUrl = uploadResult.Value;

            await unitOfWork.SaveAsync();

            return Result<string>.Success(group.ImageUrl);
        }
    }
}
