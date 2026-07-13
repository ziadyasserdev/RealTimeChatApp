using MediatR;
using Microsoft.AspNetCore.Http;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Features.Stories.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Commands.CreateVideoStory
{
    public record CreateVideoStoryCommand(
       IFormFile Video,
       string? Caption
   ) : IRequest<Result<StoryNotifierDto>>;
}
