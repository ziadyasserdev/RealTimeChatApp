using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.Groups.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Queries.GetGroupDetails
{
    public class GetGroupDetailsHandler
        : IRequestHandler<GetGroupDetailsQuery, Result<GroupDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public GetGroupDetailsHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<GroupDetailsDto>> Handle(
            GetGroupDetailsQuery request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
                return Result<GroupDetailsDto>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");

            var userId = _currentUser.UserId!;

            var isMember = await _unitOfWork.GroupMembers
                .Query()
                .AnyAsync(x =>
                    x.GroupId == request.GroupId &&
                    x.UserId == userId,
                    cancellationToken);

            if (!isMember)
                return Result<GroupDetailsDto>.Failure(
                    ResultStatus.Forbidden,
                    "You are not a member of this group.");

            var group = await _unitOfWork.Groups
                .Query()
                .AsNoTracking()
                .Include(x => x.CreatedBy)
                .Include(x => x.Members)
                .FirstOrDefaultAsync(
                    x => x.Id == request.GroupId,
                    cancellationToken);

            if (group is null)
                return Result<GroupDetailsDto>.Failure(
                    ResultStatus.NotFound,
                    "Group not found.");

            var dto = new GroupDetailsDto
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
                ImageUrl = group.ImageUrl,
                IsPrivate = group.IsPrivate,
                MembersCount = group.Members.Count,
                MaxMembers = group.MaxMembers,
                CreatedById = group.CreatedById,
                CreatedByName = group.CreatedBy.UserName!,
                CreatedAt = group.CreatedAt,

            
                InviteCode = group.CreatedById == userId
                    ? group.InviteCode
                    : null
            };

            return Result<GroupDetailsDto>.Success(dto);
        }
    }
}
