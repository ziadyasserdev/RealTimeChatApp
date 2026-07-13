using MediatR;
using Microsoft.AspNetCore.Http;
using RealTimeChatApp.Application.Commons.Results;
using RealTimeChatApp.Application.Features.Stories.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Stories.Commands.CreateImageStory
{
    public record CreateImageStoryCommand(
     IFormFile Image,
     string? Caption
 ) : IRequest<Result<StoryNotifierDto>>;
    public static class CloudinaryFolders
    {
        public const string Stories = "stories";

        public const string Profiles = "profiles";

        public const string Groups = "groups";
    }
}
