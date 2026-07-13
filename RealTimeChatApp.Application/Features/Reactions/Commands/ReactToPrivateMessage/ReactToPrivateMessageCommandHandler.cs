using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Contracts.Identity;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.Reactions.Dtos;
using RealTimeChatApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Reactions.Commands.ReactToPrivateMessage
{
    public class ReactToPrivateMessageCommandHandler
      : IRequestHandler<
          ReactToPrivateMessageCommand,
          Result<ReactionNotifierDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public ReactToPrivateMessageCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<ReactionNotifierDto>> Handle(
            ReactToPrivateMessageCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Result<ReactionNotifierDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized.");
            }

            var userId = _currentUser.UserId!;

            var message = await _unitOfWork.PrivateMessages
                .Query()
                .Include(x => x.Reactions)
                .FirstOrDefaultAsync(
                    x => x.Id == request.MessageId,
                    cancellationToken);

            if (message is null)
            {
                return Result<ReactionNotifierDto>.Failure(
                    ResultStatus.NotFound,
                    "Message not found.");
            }

            if (message.IsDeletedForEveryone)
            {
                return Result<ReactionNotifierDto>.Failure(
                    ResultStatus.Conflict,
                    "Message has been deleted.");
            }

            if (message.SenderId != userId &&
                message.ReceiverId != userId)
            {
                return Result<ReactionNotifierDto>.Failure(
                    ResultStatus.Forbidden,
                    "You are not allowed to react to this message.");
            }

            var reaction = message.Reactions
                .FirstOrDefault(x => x.UserId == userId);

            bool removed = false;

            if (reaction is null)
            {
                reaction = new Reaction
                {
                    UserId = userId,
                    PrivateMessageId = message.Id,
                    ReactionType = request.ReactionType,
                    ReactedAt = DateTime.Now,
                    CreatedAt = DateTime.Now
                };

                await _unitOfWork.Reactions.AddAsync(reaction);
            }
            else
            {
             
                if (reaction.ReactionType == request.ReactionType)
                {
                    _unitOfWork.Reactions.Delete(reaction);

                    removed = true;
                }
                else
                {
                   
                    reaction.ReactionType = request.ReactionType;
                    reaction.ReactedAt = DateTime.Now;
                    reaction.ModifiedAt = DateTime.Now;
                }
            }

            await _unitOfWork.SaveAsync();

            return Result<ReactionNotifierDto>.Success(
                new ReactionNotifierDto
                {
                    MessageId = message.Id,

                    UserId = userId,

                    SenderId = message.SenderId,

                    ReceiverId = message.ReceiverId,

                    ReactionType = removed
                        ? null
                        : reaction.ReactionType,
                    UserName = _currentUser.UserName!,
                    GroupId = null,

                    Removed = removed
                });
        }
    }
}
