using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Doctors.Commands.ActivateReceptionist;
using GraduationProject.Application.Features.Doctors.Commands.AssignSupervisor;
using GraduationProject.Application.Features.Doctors.Commands.CreateDoctorBreak;
using GraduationProject.Application.Features.Doctors.Commands.CreateReceptionist;
using GraduationProject.Application.Features.Doctors.Commands.DeactivateReceptionist;
using GraduationProject.Application.Features.Doctors.Commands.UpdateDoctorProfile;
using GraduationProject.Application.Features.Doctors.Dtos;
using GraduationProject.Application.Features.Doctors.Queries.GetDoctorDashboard;
using GraduationProject.Application.Features.Doctors.Queries.GetDoctorProfile;
using GraduationProject.Application.Features.Doctors.Queries.GetDoctorProfileForPatient;
using GraduationProject.Application.Features.Doctors.Queries.GetDoctorStatistics;
using GraduationProject.Application.Features.Doctors.Queries.GetDoctorTodaySummary;
using GraduationProject.Application.Features.Doctors.Queries.GetInsights;
using GraduationProject.Application.Features.Doctors.Queries.GetMyStudentDoctors;
using GraduationProject.Application.Features.Doctors.Queries.GetPatientGrowth;
using GraduationProject.Application.Features.Doctors.Queries.GetPatientInfo;
using GraduationProject.Application.Features.Doctors.Queries.GetPatientMedicalHistory;
using GraduationProject.Application.Features.Doctors.Queries.GetPatients;
using GraduationProject.Application.Features.Doctors.Queries.GetPendingScans;
using GraduationProject.Application.Features.Doctors.Queries.GetRecentPatients;
using GraduationProject.Application.Features.Doctors.Queries.GetRecentReports;
using GraduationProject.Application.Features.Doctors.Queries.GetReceptionistAccessControl;
using GraduationProject.Application.Features.Doctors.Queries.GetSupervisingRequestById;
using GraduationProject.Application.Features.Doctors.Queries.GetTodayAppointments;
using GraduationProject.Application.Features.Doctors.Queries.GetWeeklySchedule;
using GraduationProject.Application.Features.Patients.Dtos;
using GraduationProject.Application.Features.Scans.Dtos;
using GraduationProject.Data.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

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
    Summary = "Get doctor profile for patient",
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

        [HttpPost("add-receptionist")]
        [Authorize(Roles = "Doctor")]
        [SwaggerOperation(
   Summary = "Add receptionist",
   Description = "Creates a new receptionist account for the logged-in doctor."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddReceptionist([FromForm] CreateReceptionistCommand command)
        {

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpGet("insights")]
        [SwaggerOperation(
    Summary = "Get doctor insights",
    Description = "Returns dashboard statistics for the logged-in doctor (patients, appointments, scans, revenue)."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetInsights()
        {
            var result = await mediator.Send(new GetInsightsQuery());
            return result.ToActionResult();
        }

        [HttpGet("patient-growth")]
        [SwaggerOperation(
    Summary = "Get patient growth",
    Description = "Returns monthly patient growth for dashboard charts."
)]
        public async Task<IActionResult> GetPatientGrowth()
        {
            var result = await mediator.Send(new GetPatientGrowthQuery());
            return result.ToActionResult();
        }

        [HttpGet("recent-reports")]
        [SwaggerOperation(
            Summary = "Get recent medical reports",
            Description = "Returns the latest medical reports for the authenticated doctor including scans, medical records, and AI reports in a unified timeline sorted by most recent first."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRecentReports()
        {
            var result = await mediator.Send(new GetRecentReportsQuery());
            return result.ToActionResult();
        }
        [HttpGet("my-students")]
        [SwaggerOperation(
    Summary = "Get my student doctors",
    Description = "Retrieves all student doctors assigned to the currently authenticated doctor."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetMyStudentDoctors()
        {
            var query = new GetMyStudentDoctorsQuery();

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }
        [HttpGet("supervising-requests/{id}")]
        [SwaggerOperation(
    Summary = "Get supervising request by id",
    Description = "Retrieves supervising request details by request id."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSupervisingRequestById([FromRoute] int id)
        {
            var query = new GetSupervisingRequestByIdQuery(id);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }
        [HttpPost("assign-supervisor")]
        [SwaggerOperation(
    Summary = "Assign supervisor",
    Description = "Assigns a supervisor to a student doctor."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AssignSupervisor([FromBody] AssignSupervisorCommand command)
        {
            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpGet("My-Receptionst")]
        [SwaggerOperation(
        Summary = "Get receptionist access control dashboard",
        Description = "Returns receptionist statistics and receptionist list for the current doctor."
    )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReceptionistAccessControl()
        {
            var result = await mediator.Send(
                new GetReceptionistAccessControlQuery());

            return result.ToActionResult();
        }
        [HttpPatch("{id}/activate")]
        [SwaggerOperation(
         Summary = "Activate receptionist",
         Description = "Activates receptionist account."
     )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Activate(int id)
        {
            var result = await mediator.Send(
                new ActivateReceptionistCommand(id));

            return result.ToActionResult();
        }
        [HttpPatch("{id}/deactivate")]
        [SwaggerOperation(
    Summary = "Deactivate receptionist",
    Description = "Deactivates a receptionist account."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await mediator.Send(
                new DeactivateReceptionistCommand(id));

            return result.ToActionResult();
        }
        [HttpPost("time-request-off")]
        [SwaggerOperation(
    Summary = "Create doctor break",
    Description = "Creates a temporary break for a doctor where no appointments can be booked."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateDoctorBreakCommand command)
        {
            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
    }
}
