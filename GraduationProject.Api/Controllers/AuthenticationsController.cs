using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Features.Auth.Commands.ConfirmEmail;
using GraduationProject.Application.Features.Auth.Commands.ForgetPassword;
using GraduationProject.Application.Features.Auth.Commands.LockUser;
using GraduationProject.Application.Features.Auth.Commands.Login;
using GraduationProject.Application.Features.Auth.Commands.ResetPassword;
using GraduationProject.Application.Features.Auth.Commands.UnlockUser;
using GraduationProject.Application.Features.Doctors.Commands.CreateDoctor;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GraduationProject.Api.Controllers
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
        [HttpPost("Register-Doctor")]
        public async Task<IActionResult> CreateDoctor(CreateDoctorCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPost("Verify-Email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailCommand verifyEmailCommand)
        {
            var result = await mediator.Send(verifyEmailCommand);
            return result.ToActionResult();
        }

        [HttpPost("Submit-Doctor-Verification")]
        public async Task<IActionResult> SubmitDoctorVerification([FromForm] SubmitDoctorVerificationCommand submitDoctorVerificationCommand)
        {
            var result = await mediator.Send(submitDoctorVerificationCommand);
            return result.ToActionResult();
        }
        [HttpPost("Login")] 
        public async Task<IActionResult> Login([FromBody] LoginUserCommand loginUserCommand)
        {
            var result=await mediator.Send(loginUserCommand);
            return result.ToActionResult();
        }
        [HttpPost("forget-password")]
        [SwaggerOperation(
      Summary = "Send password reset link",
      Description = "Send a password reset link to the user's email if the account exists."
  )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordCommand command)
        {
            if (command == null || string.IsNullOrWhiteSpace(command.Email))
                return BadRequest("Email is required.");

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPost("reset-password")]
        [SwaggerOperation(
          Summary = "Reset user password",
          Description = "Reset the user's password using the token sent by email."
      )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            if (command == null || string.IsNullOrWhiteSpace(command.Email))
                return BadRequest("Email is required.");

            if (string.IsNullOrWhiteSpace(command.Token))
                return BadRequest("Reset token is required.");

            if (string.IsNullOrWhiteSpace(command.NewPassword))
                return BadRequest("New password is required.");

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpGet("confirm-email")]
        [SwaggerOperation(
   Summary = "Confirm user email",
   Description = "Confirms the user's email address using the provided userId and confirmation token."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId,
 [FromQuery] string token)
        {
            var result = await mediator.Send(
        new ConfirmEmailCommand
        {
            UserId = userId,
            Token = token
        });
            return result.ToActionResult();
        }

        [HttpPost("{userId}/lock")]
        [SwaggerOperation(
  Summary = "Lock a user account",
  Description = "Locks the specified user account, preventing the user from logging in. Accessible only by Admins or Librarians."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> LockUser(
  [SwaggerParameter(Description = "The unique identifier of the user to lock.")]
    string userId)
        {
            var result = await mediator.Send(new LockUserCommand(userId));

            return result.ToActionResult();
        }


        [HttpPost("{userId}/unlock")]
        [SwaggerOperation(
  Summary = "Unlock a user account",
  Description = "Unlocks the specified user account, allowing the user to log in again. Accessible only by Admins or Librarians."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UnlockUser(
  [SwaggerParameter(Description = "The unique identifier of the user to unlock.")]
    string userId)
        {
            var result = await mediator.Send(new UnlockUserCommand(userId));

            return result.ToActionResult();
        }
    }


}
