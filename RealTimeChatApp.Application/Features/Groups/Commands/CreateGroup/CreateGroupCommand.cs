using MediatR;
using RealTimeChatApp.Application.Commons.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Commands.CreateGroup
{
    public class CreateGroupCommand : IRequest<Result<int>>
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }



        public bool IsPrivate { get; set; }

        public int MaxMembers { get; set; }
    }
}
