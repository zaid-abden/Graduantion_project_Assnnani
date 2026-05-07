using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Features.StudentDoctors.Commands.CompleteStudentDoctorProfile;
using GraduationProject.Application.Features.StudentDoctors.Commands.CreateStudentDoctor;
using GraduationProject.Application.Features.StudentDoctors.Commands.VerifyStudentDoctorEmail;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentDoctorController : ControllerBase
    {
        private readonly IMediator mediator;

        public StudentDoctorController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("register")]
        [SwaggerOperation(
          Summary = "Create Student Doctor Account",
          Description = "Registers a new student doctor and sends email verification code"
      )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromForm] CreateStudentDoctorCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPost("verify-email")]
        [SwaggerOperation(
          Summary = "Verify Student Doctor Email",
          Description = "Verifies email using code sent to user"
      )]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyStudentDoctorEmailCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPost("complete-profile")]
        [SwaggerOperation(
          Summary = "Complete Student Doctor Profile",
          Description = "Adds NationalId, YearsOfStudy and assigns doctor using SupervisingNumber"
      )]
        public async Task<IActionResult> CompleteProfile([FromForm] CompleteStudentDoctorProfileCommand completeStudentDoctorProfileCommand)
        {
            var result = await mediator.Send(completeStudentDoctorProfileCommand);
            return result.ToActionResult();
        }

    }
}
