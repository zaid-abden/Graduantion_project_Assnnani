using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Doctors.Queries.GetDoctorById;
using GraduationProject.Application.Features.Patients.commands.updatepationtcommand;
using GraduationProject.Application.Features.Patients.Queries.DoctorByID;
using GraduationProject.Application.Features.Patients.Queries.DoctorFilteraion;
using GraduationProject.Application.Features.Patients.Queries.GetPatientProfile;
using GraduationProject.Data.Models;



//using GraduationProject.Application.Features.Patients.Commands.CreatePatient;

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PatientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[HttpPost("Register-Patient")]
        //public async Task<IActionResult> CreatePatient(CreatePatientCommand createPatientCommand)
        //{
        //    var result=await mediator.Send(createPatientCommand);
        //    return Ok(result);
        //}
        
       
        [HttpPut("Patient/Profile/")]
        public async Task<IActionResult> EditPatientById([FromBody] updatepationtcommand dto)
        {
            Result<string> result = await _mediator.Send(dto);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            Result<DoctorById_Dto> result = await _mediator.Send(new GetDoctorByIdQuery { DoctorId = id });        

            return result.ToActionResult();
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            // 🔥 هنا المفروض تجيب UserId من JWT
            var userId = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _mediator.Send(new GetPatientProfileQuery { UserId = userId });
            
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
