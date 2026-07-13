using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.PrivateMessages.Dtos;
using RealTimeChatApp.Domain.Enums;
using RealTimeChatApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.PrivateMessages.Queries.GetChats
{
    public class GetChatsQueryHandler
     : IRequestHandler<GetChatsQuery, Result<List<ChatItemDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public GetChatsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<List<ChatItemDto>>> Handle(
            GetChatsQuery request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Result<List<ChatItemDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized.");
            }

            var currentUserId = _currentUser.UserId!;

            var messages = await _unitOfWork.PrivateMessages
                .Query()
                .AsNoTracking()
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .Where(x =>

                    (x.SenderId == currentUserId || x.ReceiverId == currentUserId)

                    &&

                    !(x.SenderId == currentUserId && x.DeletedForSender)

                    &&

                    !(x.ReceiverId == currentUserId && x.DeletedForReceiver)

                )
                .OrderByDescending(x => x.SentAt)
                .ToListAsync(cancellationToken);

            var chats = messages

                .GroupBy(x =>
                    x.SenderId == currentUserId
                        ? x.ReceiverId
                        : x.SenderId)

                .Select(group =>
                {
                    var lastMessage = group
                        .OrderByDescending(x => x.SentAt)
                        .First();

                    var otherUser =
                        lastMessage.SenderId == currentUserId
                        ? lastMessage.Receiver
                        : lastMessage.Sender;

                    return new ChatItemDto
                    {
                        UserId = otherUser.Id,

                        UserName = otherUser.DisplayName,

                        ImageUrl = otherUser.ProfilePictureUrl,

                        IsOnline = otherUser.IsOnline,
                        
                        LastSeenAt = otherUser.LastSeenAt,

                        LastMessage = GetMessagePreview(lastMessage),

                        LastMessageType = lastMessage.MessageType,

                        LastMessageDate = lastMessage.SentAt,

                        UnreadCount = group.Count(x =>
                            x.ReceiverId == currentUserId &&
                            !x.IsRead)
                    };
                })

                .OrderByDescending(x => x.LastMessageDate)

                .ToList();

            return Result<List<ChatItemDto>>
                .Success(chats);
        }

        private static string GetMessagePreview(
            PrivateMessage message)
        {
            if (message.IsDeletedForEveryone)
                return "This message was deleted.";

            return message.MessageType switch
            {
                MessageType.Text => message.Content,

                MessageType.Image => "Photo",

                MessageType.Video => "Video",

              

                MessageType.File => "File",

                MessageType.Deleted => "This message was deleted.",

                _ => message.Content
            };
        }
    }
}
