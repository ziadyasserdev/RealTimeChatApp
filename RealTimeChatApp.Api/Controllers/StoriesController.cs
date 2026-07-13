using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RealTimeChatApp.Api.Commons.Responses;
using RealTimeChatApp.Api.Hubs;
using RealTimeChatApp.Api.SignalR.NotifierService;
using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Application.Features.Stories.Commands.CreateTextStory;
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
    }
}
