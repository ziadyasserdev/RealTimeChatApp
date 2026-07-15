using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeChatApp.Api.Commons.Responses;
using RealTimeChatApp.Application.Features.Authentications.Commands.ChangePassword;
using RealTimeChatApp.Application.Features.Authentications.Commands.ForgotPassword;
using RealTimeChatApp.Application.Features.Authentications.Commands.Login;
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
        [HttpPost("login")]
        [SwaggerOperation(
          Summary = "Login user",
          Description = "Authenticate a user with email and password and return an access token."
      )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await mediator.Send(command);
            
            return result.ToActionResult();
        }
        [HttpPost("change-password")]
        [Authorize]
        [SwaggerOperation(
    Summary = "Change user password",
    Description = "Allows the authenticated user to change their password by providing the current password and a new password."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpPost("forgot-password")]
        [SwaggerOperation(
    Summary = "Forgot password",
    Description = "Sends a password reset link to the user's registered email address."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
    }
}
