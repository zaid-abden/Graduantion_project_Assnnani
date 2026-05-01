using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Doctors.Commands.UpdateDoctorProfile;
using GraduationProject.Application.Features.Doctors.Dtos;
using GraduationProject.Application.Features.Doctors.Queries.GetDoctorDashboard;
using GraduationProject.Application.Features.Doctors.Queries.GetDoctorProfile;
using GraduationProject.Application.Features.Doctors.Queries.GetDoctorProfileForPatient;
using GraduationProject.Application.Features.Doctors.Queries.GetDoctorStatistics;
using GraduationProject.Application.Features.Doctors.Queries.GetDoctorTodaySummary;
using GraduationProject.Application.Features.Doctors.Queries.GetPatientInfo;
using GraduationProject.Application.Features.Doctors.Queries.GetPatientMedicalHistory;
using GraduationProject.Application.Features.Doctors.Queries.GetPatients;
using GraduationProject.Application.Features.Doctors.Queries.GetPendingScans;
using GraduationProject.Application.Features.Doctors.Queries.GetRecentPatients;
using GraduationProject.Application.Features.Doctors.Queries.GetTodayAppointments;
using GraduationProject.Application.Features.Doctors.Queries.GetWeeklySchedule;
using GraduationProject.Application.Features.Patients.Dtos;
using GraduationProject.Application.Features.Scans.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GraduationProject.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	//[Authorize]
	public class DoctorsController : ControllerBase
	{
		private readonly IMediator mediator;
		public DoctorsController(IMediator mediator)
		{
			this.mediator = mediator;
		}

        [HttpGet("dashboard")]
        [Authorize(Roles = "Doctor")]
        [SwaggerOperation(
          Summary = "Doctor dashboard",
          Description = "Returns statistics for doctor dashboard including appointments, patients, scans, and satisfaction rate."
      )]
        [ProducesResponseType(typeof(Result<DoctorDashboardDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDashboard()
        {
            var query = new GetDoctorDashboardQuery();

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }
        [HttpGet("today")]
        [Authorize(Roles = "Doctor")]
        [SwaggerOperation(
    Summary = "Get today's appointments",
    Description = "Returns all appointments scheduled for today for the logged-in doctor."
)]
        [ProducesResponseType(typeof(Result<List<TodayAppointmentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetTodayAppointments()
        {
            var result = await mediator.Send(new GetTodayAppointmentsQuery());

            return result.ToActionResult();
        }
        [HttpGet("pending-scans")]
        [Authorize(Roles = "Doctor")]
        [SwaggerOperation(
          Summary = "Get pending scans",
          Description = "Returns all pending scans that need review by the logged-in doctor."
      )]
        [ProducesResponseType(typeof(Result<List<ScanDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetPendingScans()
        {
            var result = await mediator.Send(new GetPendingScansQuery());

            return result.ToActionResult();
        }

        [HttpGet("recent-patients")]
        [Authorize(Roles = "Doctor")]
        [SwaggerOperation(
        Summary = "Get recent patients",
        Description = "Returns the most recent patients for the logged-in doctor."
    )]
        [ProducesResponseType(typeof(Result<List<RecentPatientDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetRecentPatients([FromQuery] int count = 5)
        {
            var result = await mediator.Send(new GetRecentPatientsQuery
            {
                Count = count
            });

            return result.ToActionResult();
        }

        [HttpGet("patients")]
        [SwaggerOperation(
         Summary = "Get patients for doctor dashboard",
         Description = "Returns paginated list of patients with optional filtering and search."
     )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetPatients([FromQuery] GetPatientsQuery query)
        {
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("patient-info/{id}")]
        [SwaggerOperation(
        Summary = "Get patient details",
        Description = "Returns full patient details for doctor dashboard."
    )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetPatientInfo(int id)
        {
            var result = await mediator.Send(new GetPatientInfoQuery(id));
            return result.ToActionResult();

        }
        [HttpGet("patient-medical-history/{patientId}")]
        public async Task<IActionResult> GetPatientMedicalHistory(int patientId)
        {
            var query = new GetPatientMedicalHistoryQuery { PatientId = patientId };
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("weekly-schedule")]
        [SwaggerOperation(
    Summary = "Get doctor's weekly schedule",
    Description = "Retrieves the weekly schedule for the current doctor."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWeeklySchedule()
        {
            var result = await mediator.Send(new GetWeeklyScheduleQuery());
            return result.ToActionResult();
        }

        [HttpGet("{doctorId}")]
        [SwaggerOperation(
    Summary = "Get doctor profile",
    Description = "Returns full doctor profile details for patient view."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDoctorProfileForPatient(int doctorId)
        {
            var result = await mediator.Send(new GetDoctorProfileForPatientQuery(doctorId));
            return result.ToActionResult();
        }
    }
}
