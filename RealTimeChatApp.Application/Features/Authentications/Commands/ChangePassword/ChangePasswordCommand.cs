using MediatR;
using RealTimeChatApp.Application.Commons.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Authentications.Commands.ChangePassword
{

    public  record ChangePasswordCommand(
        string CurrentPassword,
        string NewPassword,
        string ConfirmPassword
    ) : IRequest<Result<string>>;
}
