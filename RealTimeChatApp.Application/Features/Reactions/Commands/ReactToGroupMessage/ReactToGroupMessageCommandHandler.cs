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

namespace RealTimeChatApp.Application.Features.Reactions.Commands.ReactToGroupMessage
{
    public class ReactToGroupMessageCommandHandler
    : IRequestHandler<
        ReactToGroupMessageCommand,
        Result<ReactionNotifierDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public ReactToGroupMessageCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<ReactionNotifierDto>> Handle(
            ReactToGroupMessageCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Result<ReactionNotifierDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized.");
            }

            var userId = _currentUser.UserId!;

            var message = await _unitOfWork.GroupMessages
                .Query()
                .Include(x => x.Group)
                .Include(x => x.Sender)
                .FirstOrDefaultAsync(
                    x => x.Id == request.MessageId,
                    cancellationToken);

            if (message is null)
            {
                return Result<ReactionNotifierDto>.Failure(
                    ResultStatus.NotFound,
                    "Message not found.");
            }

            var isMember = await _unitOfWork.GroupMembers
                .Query()
                .AnyAsync(x =>
                    x.GroupId == message.GroupId &&
                    x.UserId == userId,
                    cancellationToken);

            if (!isMember)
            {
                return Result<ReactionNotifierDto>.Failure(
                    ResultStatus.Forbidden,
                    "You are not a member of this group.");
            }

            var reaction = await _unitOfWork.Reactions
                .Query()
                .FirstOrDefaultAsync(x =>
                    x.GroupMessageId == request.MessageId &&
                    x.UserId == userId,
                    cancellationToken);

            bool removed = false;

            if (reaction is null)
            {
                reaction = new Reaction
                {
                    GroupMessageId = request.MessageId,
                    UserId = userId,
                    ReactionType = request.ReactionType,
                    ReactedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
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
                    reaction.ReactedAt = DateTime.UtcNow;
                    reaction.ModifiedAt = DateTime.UtcNow;

                    _unitOfWork.Reactions.Update(reaction);
                }
            }

            await _unitOfWork.SaveAsync();

            var dto = new ReactionNotifierDto
            {
                GroupId = message.GroupId,
                MessageId = message.Id,
                UserId = userId,
                UserName = _currentUser.UserName!,
                ReactionType = request.ReactionType,
                Removed = removed
            };

            return Result<ReactionNotifierDto>.Success(dto);
        }
    }
}
