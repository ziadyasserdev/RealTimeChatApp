using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.GroupMessages.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.GroupMessages.Queries.SearchGroupMessages
{
    public class SearchGroupMessagesQueryHandler
    : IRequestHandler<
        SearchGroupMessagesQuery,
        Result<List<GroupMessageSearchDto>>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;


        public SearchGroupMessagesQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }



        public async Task<Result<List<GroupMessageSearchDto>>> Handle(
            SearchGroupMessagesQuery request,
            CancellationToken cancellationToken)
        {

            if (!_currentUser.IsAuthenticated)
            {
                return Result<List<GroupMessageSearchDto>>
                    .Failure(
                        ResultStatus.Unauthorized,
                        "Unauthorized.");
            }



            var userId = _currentUser.UserId!;



         

            var isMember =
                await _unitOfWork.GroupMembers.Query()
                .AnyAsync(
                    x =>
                    x.GroupId == request.GroupId &&
                    x.UserId == userId,
                    cancellationToken);



            if (!isMember)
            {
                return Result<List<GroupMessageSearchDto>>
                    .Failure(
                        ResultStatus.Forbidden,
                        "You are not member of this group.");
            }




            var messages =
                await _unitOfWork.GroupMessages.Query()


                .Where(x =>
                    x.GroupId == request.GroupId &&
                    x.Content.Contains(request.Keyword)
                )


                .Include(x => x.Sender)


                .OrderByDescending(x => x.SentAt)


                .Select(x => new GroupMessageSearchDto
                {

                    MessageId = x.Id,

                    GroupId = x.GroupId,


                    SenderId = x.SenderId,


                    SenderName =
                        x.Sender.UserName!,


                    Content = x.Content,


                    SentAt = x.SentAt

                })


                .ToListAsync(cancellationToken);




            return Result<List<GroupMessageSearchDto>>
                .Success(messages);
        }
    }
}
