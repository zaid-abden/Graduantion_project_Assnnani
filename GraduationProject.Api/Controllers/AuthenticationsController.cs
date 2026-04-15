using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Features.Auth.Commands.Login;
using GraduationProject.Application.Features.Doctors.Commands.CreateDoctor;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
