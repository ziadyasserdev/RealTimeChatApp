using MediatR;
using RealTimeChatApp.Application.Commons.PaginatedResults;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Features.Groups.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Queries.GetGroupMessages
{
    public record GetGroupMessagesQuery(
      int GroupId,
      int Page = 1,
      int PageSize = 20
  ) : IRequest<Result<PaginatedResult<GroupMessagesDto>>>;
    public class GroupMessagesDto
    {
        public int Id { get; set; }

        public string SenderId { get; set; } = default!;

        public string SenderName { get; set; } = default!;

        public string Content { get; set; } = default!;

        public string MessageType { get; set; } = default!;

        public bool IsEdited { get; set; }

        public bool IsPinned { get; set; }

        public DateTime SentAt { get; set; }
    }
}
