using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Patients.commands.updatepationtcommand;
using GraduationProject.Application.Features.Patients.Queries.DoctorFilteraion;


//using GraduationProject.Application.Features.Patients.Commands.CreatePatient;
using GraduationProject.Application.Features.Patients.Queries.GetAllPatients;
using GraduationProject.Application.Features.Patients.Queries.GetPatientById;
using MediatR;
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

        //[HttpPost("Register-Patient")]
        //public async Task<IActionResult> CreatePatient(CreatePatientCommand createPatientCommand)
        //{
        //    var result=await mediator.Send(createPatientCommand);
        //    return Ok(result);
        //}
        [HttpGet("Get-All-Patients")]
        public async Task<IActionResult> GetAllPatients()
        {
            var result = await mediator.Send(new GetAllPatientsQuery());
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById([FromRoute] int id)
        {
            var result = await mediator.Send(new GetPatientByIdQuery(id));
            return result.ToActionResult();
        }
        [HttpPut("Patient/Profile/")]
        public async Task<IActionResult> EditPatientById([FromBody] updatepationtcommand dto)
        {
            var result = await mediator.Send(dto);
            return result.ToActionResult();
        }

        //[HttpGet("Get/Doctors")]
        //public async Task<IActionResult> GetDoctor_Filteration([FromQuery] DoctorFilterationQuery query)
        //{
        //    Result<PagedResult<DoctorFDTO>> result = await mediator.Send(query);
        //    return result.ToActionResult();
        //}
    }
}
