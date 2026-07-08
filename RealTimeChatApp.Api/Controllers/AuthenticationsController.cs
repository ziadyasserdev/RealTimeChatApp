using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeChatApp.Api.Commons.Responses;
using RealTimeChatApp.Application.Features.Authentications.Commands.Register;
using Swashbuckle.AspNetCore.Annotations;

namespace RealTimeChatApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthenticationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("register")]
        [SwaggerOperation(
        Summary = "Register a new user",
        Description = "Create a new user account with full name, email, password, and other required details."
    )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
    }
}
