using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Features.Doctors.Commands.CreateDoctor;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IMediator mediator;
        public DoctorsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("Register-Doctor")]
        public async Task<IActionResult> CreateDoctor( CreateDoctorCommand command)
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
    }
}
