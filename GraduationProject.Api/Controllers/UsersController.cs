using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Features.ApplicationUsers.Queries.GetUserProfile;
using MediatR;
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

    }
}
