using MediatR;
using RealTimeChatApp.Application.Commons.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Authentications.Commands.ForgotPassword
{
    public  record ForgotPasswordCommand(
      string Email
  ) : IRequest<Result<string>>;
}
