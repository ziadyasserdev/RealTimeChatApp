using MediatR;
using RealTimeChatApp.Application.Commons.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Commands.ChangeAnnouncementMode
{
    public record ChangeAnnouncementModeCommand(
        int GroupId,
        bool Enable
    ) : IRequest<Result<string>>;
}
