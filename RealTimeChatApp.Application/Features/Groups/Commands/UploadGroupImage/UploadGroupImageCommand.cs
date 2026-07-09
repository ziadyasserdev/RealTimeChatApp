using MediatR;
using Microsoft.AspNetCore.Http;
using RealTimeChatApp.Application.Commons.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Commands.UploadGroupImage
{
    public record UploadGroupImageCommand(
    int GroupId,
    IFormFile Image
) : IRequest<Result<string>>;
}
