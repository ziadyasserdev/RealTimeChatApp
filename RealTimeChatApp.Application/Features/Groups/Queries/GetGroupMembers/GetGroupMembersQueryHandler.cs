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

namespace RealTimeChatApp.Application.Features.Groups.Queries.GetGroupMembers
{
    public class GetGroupMembersHandler
    : IRequestHandler<GetGroupMembersQuery, Result<List<GroupMemberDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public GetGroupMembersHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<List<GroupMemberDto>>> Handle(
            GetGroupMembersQuery request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
                return Result<List<GroupMemberDto>>
                    .Failure(ResultStatus.Unauthorized,
                        "User is not authenticated.");

            var userId = _currentUser.UserId!;

            var isMember = await _unitOfWork.GroupMembers
                .Query()
                .AnyAsync(x =>
                    x.GroupId == request.GroupId &&
                    x.UserId == userId,
                    cancellationToken);

            if (!isMember)
                return Result<List<GroupMemberDto>>
                    .Failure(ResultStatus.Forbidden,
                        "You are not a member of this group.");

            var members = await _unitOfWork.GroupMembers
                .Query()
                .AsNoTracking()
                .Where(x => x.GroupId == request.GroupId)
                .Include(x => x.User)
                .OrderBy(x => x.JoinedAt)
                .Select(x => new GroupMemberDto
                {
                    UserId = x.UserId,
                    UserName = x.User.UserName!,
                    Nickname = x.Nickname,
                    Role = x.Role,
                    JoinedAt = x.JoinedAt,
                    IsMuted = x.IsMuted
                })
                .ToListAsync(cancellationToken);

            return Result<List<GroupMemberDto>>
                .Success(members);
        }
    }
}
