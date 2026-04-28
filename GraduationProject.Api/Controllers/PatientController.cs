using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Doctors.Queries.GetDoctorById;
using GraduationProject.Application.Features.Patients.commands.AddPatient;
using GraduationProject.Application.Features.Patients.commands.AddPatientAllergy;
using GraduationProject.Application.Features.Patients.commands.ChangePatientStatus;
using GraduationProject.Application.Features.Patients.commands.updatepationtcommand;
using GraduationProject.Application.Features.Patients.Queries.DoctorByID;
using GraduationProject.Application.Features.Patients.Queries.DoctorFilteraion;

//using GraduationProject.Application.Features.Patients.Queries.GetAvaliableSlots;
using GraduationProject.Application.Features.Patients.Queries.GetPatientProfile;
using GraduationProject.Application.Features.Patients.Queries.PatientDashborad;




//using GraduationProject.Application.Features.Patients.Commands.CreatePatient;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
            var result = await _mediator.Send(dto);
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


            if (!int.TryParse(User.FindFirst("sub")?.Value, out int userId))
                return Unauthorized();

            var result = await _mediator.Send(new GetPatientProfileQuery { UserId = userId });

            return result.ToActionResult();
        }


        //[HttpGet("{id}/available-slots")]
        //public async Task<IActionResult> GetAvailableSlots(int id)
        //{
        //    var result = await _mediator.Send(new GetAvaliableSlotsQuery
        //    {
        //        DoctorId = id
        //    });

        //    return result.ToActionResult();
        //}

        [HttpGet("Get/Doctors")]
        public async Task<IActionResult> GetDoctor_Filteration([FromQuery] DoctorFilterationQuery query)
        {
            Result<PagedResult<DoctorFDTO>> result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpPost("register")]
        [SwaggerOperation(
             Summary = "Register new patient",
             Description = "Creates a new patient profile and links it to the authenticated user."
         )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RegisterPatient([FromBody] AddPatientCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpGet("Get/PatientDashboard/{id}")]
        public async Task<IActionResult> GetAvailableSlots(int id)
        {
            var result = await _mediator.Send(new PatientDashboardQuery(id));
            return result.ToActionResult();
        }

        [HttpPatch("change-status")]
        [SwaggerOperation(
          Summary = "Change patient status",
          Description = "Updates the status of a specific patient (e.g., Active, Inactive, Suspended)."
      )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePatientStatus([FromBody] ChangePatientStatusCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpPost("add-allergy")]
        [SwaggerOperation(
         Summary = "Add allergy to patient",
         Description = "Assigns an allergy to a specific patient with optional notes."
     )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddPatientAllergy([FromBody] AddPatientAllergyCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
