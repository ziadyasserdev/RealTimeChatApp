using MediatR;
using RealTimeChatApp.Application.Commons.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Commands.JoinGroup
{
    public class JoinGroupCommand : IRequest<Result<string>>
    {
        public int GroupId { get; set; }

        public string? InviteCode { get; set; }
    }
}
