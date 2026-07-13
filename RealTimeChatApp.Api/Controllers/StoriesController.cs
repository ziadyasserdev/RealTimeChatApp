using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RealTimeChatApp.Api.Commons.Responses;
using RealTimeChatApp.Api.Hubs;
using RealTimeChatApp.Api.SignalR.NotifierService;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.Stories.Commands.CreateImageStory;
using RealTimeChatApp.Application.Features.Stories.Commands.CreateTextStory;
using RealTimeChatApp.Application.Features.Stories.Commands.CreateVideoStory;
using Swashbuckle.AspNetCore.Annotations;

namespace RealTimeChatApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoriesController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IChatHubNotifier chatHubNotifier;
        private readonly IUnitOfWork unitOfWork;

        public StoriesController(IMediator mediator
            , IChatHubNotifier chatHubNotifier
            , IUnitOfWork unitOfWork)
        {
            this.mediator = mediator;
            this.chatHubNotifier = chatHubNotifier;
            this.unitOfWork = unitOfWork;
        }
        [HttpPost("text")]
        [SwaggerOperation(
    Summary = "Create text story",
    Description = "Creates a new text story that is visible to the user's contacts for 24 hours. A real-time notification is sent to connected clients using SignalR."
)]
        public async Task<IActionResult> CreateTextStory(
    CreateTextStoryCommand command)
        {
            var result = await mediator.Send(command);

            if (result.IsSuccess)
            {
                await chatHubNotifier.StoryCreatedAsync(result.Value!);
            }

            return result.ToActionResult();
        }
        [HttpPost("image")]
        [SwaggerOperation(
    Summary = "Create image story",
    Description = "Creates a new image story that is visible to the user's contacts for 24 hours. The image is uploaded to Cloudinary and connected clients are notified in real time using SignalR."
)]
        public async Task<IActionResult> CreateImageStory(
    [FromForm] CreateImageStoryCommand command)
        {
            var result = await mediator.Send(command);

            if (result.IsSuccess)
            {
                await chatHubNotifier.StoryCreatedAsync(result.Value!);
            }

            return result.ToActionResult();
        }
        [HttpPost("video")]
        [SwaggerOperation(
    Summary = "Create video story",
    Description = "Creates a new video story that is visible to the user's contacts for 24 hours. The video is uploaded to Cloudinary and connected clients are notified in real time using SignalR."
)]
        public async Task<IActionResult> CreateVideoStory(
    [FromForm] CreateVideoStoryCommand command)
        {
            var result = await mediator.Send(command);

            if (result.IsSuccess)
            {
                await chatHubNotifier.StoryCreatedAsync(result.Value!);
            }

            return result.ToActionResult();
        }
    }
}
