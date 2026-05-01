using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.ApplicationUsers.Commands.AddImageProfile;
using GraduationProject.Application.Features.ApplicationUsers.Commands.ChangePassword;
using GraduationProject.Application.Features.ApplicationUsers.Commands.DeleteImageProfile;
using GraduationProject.Application.Features.ApplicationUsers.Commands.DeleteMyAccount;
using GraduationProject.Application.Features.ApplicationUsers.Commands.UpdateImageProfile;
using GraduationProject.Application.Features.ApplicationUsers.Queries.GetUserProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;

        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet("my-profile")]
        [SwaggerOperation(
        Summary = "Get current user's profile",
        Description = "Retrieve the profile details of the authenticated user."
    )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserProfile()
        {
            var result = await mediator.Send(new GetUserProfileQuery { });
            return result.ToActionResult();
        }


        [HttpPost("change-password")]
        [SwaggerOperation(
            Summary = "Change user password",
            Description = "Allow a user to change their password by providing old and new password."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand changePasswordCommand)
        {
            var result = await mediator.Send(changePasswordCommand);
            return result.ToActionResult();
        }

        [HttpPost("upload-image-profile")]
        [Authorize]
        [SwaggerOperation(
           Summary = "Upload  user profile image",
           Description = "Allows the authenticated user to upload or update their profile image. " +
                         "The image must be JPG, JPEG, or PNG and should not exceed 2MB."
       )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<string>))]       // Image uploaded successfully
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<string>))] // Validation errors (null file, invalid type, size)
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<string>))] // Not logged in
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<string>))] // User not found
        public async Task<IActionResult> AddProfileImage([FromForm] AddImageProfileCommand addImageProfileCommand)
        {
            var result = await mediator.Send(addImageProfileCommand);
            return result.ToActionResult();
        }

        [HttpPatch("update-image-profile")]

        [SwaggerOperation(
      Summary = "Update the authenticated user's profile image",
      Description = "Allows the logged-in user to update their profile image. " +
                    "Accepted file types: JPG, JPEG, PNG. Maximum size: 2MB."
  )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<string>))]       // Image updated successfully
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<string>))] // Validation error (null file, invalid type, size)
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<string>))] // User not authenticated
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<string>))] // User not found
        public async Task<IActionResult> UpdateImageProfile([FromForm] UpdateImageProfileCommand command)
        {
            if (command.ProfileImage is null)
                return Result<string>.Failure(ResultStatus.ValidationError, "Profile image is required.").ToActionResult();

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpDelete("image-profile")]
        [Authorize]
        [SwaggerOperation(
    Summary = "Delete authenticated user's profile image",
    Description = "Allows the logged-in user to delete their profile image. " +
                  "This does not delete the user account, only the profile image."
)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<string>))]       // Image deleted successfully
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<string>))] // Validation error / image not found
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<string>))] // User not authenticated
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<string>))] // User or image not found
        public async Task<IActionResult> DeleteImageProfile()
        {
            var result = await mediator.Send(new DeleteImageProfileCommand());
            return result.ToActionResult();
        }

        [HttpDelete("my-account")]
        [Authorize]
        [SwaggerOperation(
      Summary = "Delete own user account",
      Description = "Allows the authenticated user to permanently delete their own account."
  )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteMyAccount()
        {
            var result = await mediator.Send(new DeleteMyAccountCommand());

            return result.ToActionResult();
        }
    }
}
