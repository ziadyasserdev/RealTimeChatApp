using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.PrivateMessages.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.PrivateMessages.Queries.SearchPrivateMessages
{
    public class SearchPrivateMessagesQueryHandler
         : IRequestHandler<
             SearchPrivateMessagesQuery,
             Result<List<PrivateMessageSearchDto>>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;


        public SearchPrivateMessagesQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }



        public async Task<Result<List<PrivateMessageSearchDto>>> Handle(
            SearchPrivateMessagesQuery request,
            CancellationToken cancellationToken)
        {


            if (!_currentUser.IsAuthenticated)
            {
                return Result<List<PrivateMessageSearchDto>>
                    .Failure(
                        ResultStatus.Unauthorized,
                        "Unauthorized.");
            }



            var userId = _currentUser.UserId!;



            var messages =
                await _unitOfWork.PrivateMessages.Query()

                .Where(x =>
                    (
                        x.SenderId == userId &&
                        x.ReceiverId == request.UserId
                    )
                    ||
                    (
                        x.SenderId == request.UserId &&
                        x.ReceiverId == userId
                    )
                )

                .Where(x =>
                    x.Content.Contains(request.Keyword)
                )


                .Include(x => x.Sender)

                .Include(x => x.Receiver)


                .OrderByDescending(x => x.SentAt)


                .Select(x => new PrivateMessageSearchDto
                {
                    MessageId = x.Id,

                    SenderId = x.SenderId,

                    SenderName =
                        x.Sender.UserName!,


                    ReceiverId = x.ReceiverId,

                    ReceiverName =
                        x.Receiver.UserName!,


                    Content = x.Content,


                    SentAt = x.SentAt
                })


                .ToListAsync(cancellationToken);



            return Result<List<PrivateMessageSearchDto>>
                .Success(messages);
        }
    }
}
