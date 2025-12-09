using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Patients.Commands.CreatePatient;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IMediator mediator;
        public PatientController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody] CreatePatientCommand createPatientCommand)
        {
            var result=await mediator.Send(createPatientCommand);
            return Ok(result);
        }
        
    }
}
