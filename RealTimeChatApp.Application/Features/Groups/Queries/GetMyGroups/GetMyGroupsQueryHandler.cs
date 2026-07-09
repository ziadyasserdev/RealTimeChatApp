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

namespace RealTimeChatApp.Application.Features.Groups.Queries.GetMyGroups
{
    public class GetMyGroupsHandler
     : IRequestHandler<GetMyGroupsQuery, Result<List<GroupListDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public GetMyGroupsHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<List<GroupListDto>>> Handle(
            GetMyGroupsQuery request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
                return Result<List<GroupListDto>>
                    .Failure(ResultStatus.Unauthorized,
                        "User is not authenticated.");

            var userId = _currentUser.UserId!;

            var groups = await _unitOfWork.GroupMembers
                .Query()
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => new GroupListDto
                {
                    Id = x.Group.Id,
                    Name = x.Group.Name,
                    Description = x.Group.Description,
                    ImageUrl = x.Group.ImageUrl,
                    IsPrivate = x.Group.IsPrivate,
                    MembersCount = x.Group.Members.Count,
                    CreatedAt = x.Group.CreatedAt
                })
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

            return Result<List<GroupListDto>>
                .Success(groups);
        }
    }
}