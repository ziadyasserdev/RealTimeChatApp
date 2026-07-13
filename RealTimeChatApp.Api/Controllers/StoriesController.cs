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
using RealTimeChatApp.Application.Features.Stories.Commands.DeleteStory;
using RealTimeChatApp.Application.Features.Stories.Commands.ViewStory;
using RealTimeChatApp.Application.Features.Stories.Queries.GetStoryFeed;
using RealTimeChatApp.Application.Features.Stories.Queries.GetStoryViewers;
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
        [HttpGet("feed")]
        [SwaggerOperation(
    Summary = "Get story feed",
    Description = "Retrieves all active stories from the authenticated user's contacts. Expired stories are excluded from the feed."
)]
        public async Task<IActionResult> GetFeed()
        {
            var result = await mediator.Send(new GetStoryFeedQuery());

            return result.ToActionResult();
        }
        [HttpPost("{storyId}/view")]
        [SwaggerOperation(
    Summary = "View story",
    Description = "Marks a story as viewed by the authenticated user. A view is recorded only once per user."
)]
        public async Task<IActionResult> ViewStory(int storyId)
        {
            var result = await mediator.Send(new ViewStoryCommand(storyId));

            return result.ToActionResult();
        }

        [HttpGet("{storyId}/viewers")]
        [SwaggerOperation(
    Summary = "Get story viewers",
    Description = "Retrieves all users who have viewed the specified story."
)]
        public async Task<IActionResult> GetStoryViewers(int storyId)
        {
            var result = await mediator.Send(new GetStoryViewersQuery(storyId));

            return result.ToActionResult();
        }
        [HttpDelete("{storyId}")]
        [SwaggerOperation(
    Summary = "Delete story",
    Description = "Deletes the specified story if it belongs to the authenticated user and notifies connected clients in real time using SignalR."
)]
        public async Task<IActionResult> DeleteStory(int storyId)
        {
            var result = await mediator.Send(
                new DeleteStoryCommand(storyId));

            if (result.IsSuccess)
            {
                await chatHubNotifier.StoryDeletedAsync(
                    result.Value!);
            }

            return result.ToActionResult();
        }
        [HttpDelete("{storyId}")]
        [SwaggerOperation(
    Summary = "Delete story",
    Description = "Deletes the specified story owned by the authenticated user and notifies connected clients in real time using SignalR."
)]
        public async Task<IActionResult> Deletestory(int storyId)
        {
            var result = await mediator.Send(
                new DeleteStoryCommand(storyId));

            if (result.IsSuccess)
            {
                await chatHubNotifier.StoryDeletedAsync(result.Value!);
            }

            return result.ToActionResult();
        }
    }
}
