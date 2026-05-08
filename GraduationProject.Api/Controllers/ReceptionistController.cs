using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Receptionists.Commands.AddToQueue;
using GraduationProject.Application.Features.Receptionists.Commands.CheckInAppointment;
using GraduationProject.Application.Features.Receptionists.Commands.CompleteAppointment;
using GraduationProject.Application.Features.Receptionists.Commands.ConfirmAppointment;
using GraduationProject.Application.Features.Receptionists.Commands.RegisterPatient;
using GraduationProject.Application.Features.Receptionists.Commands.RescheduleAppointment;
using GraduationProject.Application.Features.Receptionists.Commands.ScheduleAppointment;
using GraduationProject.Application.Features.Receptionists.Commands.StartConsultation;
//using GraduationProject.Application.Features.Receptionist.Queries.GetAllappointmentForReceptionist;
//using GraduationProject.Application.Features.Receptionist.Queries.GetAllPatients;
//using GraduationProject.Application.Features.Receptionist.Queries.PatientInfo;
//using GraduationProject.Application.Features.Receptionist.Queries.ReceptionistDashboard;
using GraduationProject.Application.Features.Receptionists.Dtos;
using GraduationProject.Application.Features.Receptionists.Queries.GetAppointmentDetails;
using GraduationProject.Application.Features.Receptionists.Queries.GetAvailableSlotsByDate;
using GraduationProject.Application.Features.Receptionists.Queries.GetPatientDoctorInfo;
using GraduationProject.Application.Features.Receptionists.Queries.GetPatientInfo;
using GraduationProject.Application.Features.Receptionists.Queries.GetPatientMedicalHistory;
using GraduationProject.Application.Features.Receptionists.Queries.GetPatientQueue;
using GraduationProject.Application.Features.Receptionists.Queries.GetPatients;
using GraduationProject.Application.Features.Receptionists.Queries.GetRecentPatients;
using GraduationProject.Application.Features.Receptionists.Queries.GetReceptionistAppointmentsDashboard;
using GraduationProject.Application.Features.Receptionists.Queries.GetReceptionistDashboard;
using GraduationProject.Application.Features.Receptionists.Queries.GetReceptionistOverview;
using GraduationProject.Application.Features.Receptionists.Queries.GetReceptionistRecentActivity;
using GraduationProject.Application.Features.Receptionists.Queries.GetscheduleAppointment;
using GraduationProject.Application.Features.Receptionists.Queries.GetTodaysAppointments;
using GraduationProject.Data.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceptionistController : ControllerBase
    {
        private readonly IMediator mediator;
        public ReceptionistController(IMediator mediator)
        {
           
            this.mediator = mediator;
        }
       
        [HttpPatch("{id}/confirm")]
        //[Authorize(Roles = "Doctor,Admin")]
        [SwaggerOperation(
   Summary = "Confirm appointment",
   Description = "Allows doctor or admin to confirm a pending appointment."
)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Confirm(int id)
        {


            var result = await mediator.Send(new ConfirmAppointmentCommand(id));

            return result.ToActionResult();
        }

        [HttpPost("checkin/{id}")]
        [SwaggerOperation(
  Summary = "Check-in appointment",
  Description = "Marks an appointment as checked-in when the patient arrives."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CheckIn(int id)
        {
            var result = await mediator.Send(new CheckInAppointmentCommand(id));

            return result.ToActionResult();
        }

        [HttpPost("start/{id}")]
        [SwaggerOperation(
    Summary = "Start consultation",
    Description = "Marks the consultation as started for the given appointment."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> StartConsultation(int id)
        {
            var result = await mediator.Send(new StartConsultationCommand(id));

            return result.ToActionResult();
        }



        [HttpPost("complete")]
        [SwaggerOperation(
    Summary = "Complete appointment",
    Description = "Marks a specific appointment as completed."
)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CompleteAppointment([FromBody] CompleteAppointmentCommand command)
        {
            //var command = new CompleteAppointmentCommand(appointmentId);

            var result = await mediator.Send(new CompleteAppointmentCommand(command.AppointmentId));

            return result.ToActionResult();
        }
        [HttpGet("dashboard")]
        [SwaggerOperation(
    Summary = "Get receptionist dashboard",
    Description = "Returns dashboard data for the logged-in receptionist."
)]
        [ProducesResponseType(typeof(Result<ReceptionistDashboardDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetReceptionistDashboard()
        {
            var result = await mediator.Send(new GetReceptionistDashboardQuery());

            return result.ToActionResult();
        }

        [HttpGet("patient-queue")]
        [SwaggerOperation(
    Summary = "Get patient queue",
    Description = "Returns the current waiting queue of patients for the receptionist."
)]
        [ProducesResponseType(typeof(Result<List<PatientQueueDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPatientQueue()
        {
            var result = await mediator.Send(new GetPatientQueueQuery());

            return result.ToActionResult();
        }

        [HttpGet("today-appointments")]
        [SwaggerOperation(
    Summary = "Get today's appointments",
    Description = "Returns all confirmed appointments for today for the logged-in receptionist."
)]
        [ProducesResponseType(typeof(Result<List<TodaysAppointmentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTodaysAppointments()
        {
            var result = await mediator.Send(new GetTodaysAppointmentsQuery());

            return result.ToActionResult();
        }
        [HttpPost("register-patient")]
        [SwaggerOperation(
    Summary = "Register new patient",
    Description = "Allows the receptionist to register a new patient in the system."
)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RegisterPatient([FromBody] RegisterPatientCommand command)
        {
            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpGet("available-slots")]
        [SwaggerOperation(
    Summary = "Get doctor's available slots by specific date",
    Description = "Returns available slots for the receptionist's doctor on a specific date."
)]
        [ProducesResponseType(typeof(Result<List<SlottsDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAvailableSlots([FromQuery] DateOnly date)
        {
            var query = new GetAvailableSlotsByDateQuery
            {
                Date = date
            };

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }

        [HttpPost("schedule-appointment")]
        [SwaggerOperation(
    Summary = "Schedule appointment",
    Description = "Allows the receptionist to schedule an appointment for a patient using an available slot."
)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ScheduleAppointment([FromBody] ScheduleAppointmentCommand command, AppointmentType appointmentType)
        {
            command.AppointmentType = appointmentType;
            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpGet("patient-info/{id}")]
        [SwaggerOperation(
    Summary = "Get patient appointment info",
    Description = "Returns detailed information about a patient's appointment for the receptionist."
)]
        [ProducesResponseType(typeof(Result<PatientInfoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPatientInfo(int id)
        {
            var query = new GetPatientInfoQuery(id);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }
        [HttpPost("add-to-queue/{appointmentId}")]
        [SwaggerOperation(
    Summary = "Add patient to queue",
    Description = "Allows receptionist to add a patient (appointment) to the queue."
)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddToQueue(int appointmentId)
        {
            var command = new AddToQueueCommand(appointmentId);

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpGet("schedule-appointment/{appointmentId}")]
        [SwaggerOperation(
    Summary = "Get scheduled appointment details",
    Description = "Returns detailed schedule information for a specific appointment for the receptionist."
)]
        [ProducesResponseType(typeof(Result<scheduleAppointmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetScheduleAppointment(int appointmentId)
        {
            var query = new GetscheduleAppointmentQuery(appointmentId);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }
        [HttpPut("reschedule-appointment")]
        [SwaggerOperation(
    Summary = "Reschedule appointment",
    Description = "Allows receptionist to reschedule an existing appointment to a new date and time slot."
)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RescheduleAppointment(RescheduleAppointmentCommand command)
        {
            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpGet("receptionist-overview")]
        [SwaggerOperation(
    Summary = "Get receptionist dashboard overview",
    Description = "Returns an overview for the receptionist dashboard including appointments, queue status, and key metrics."
)]
        [ProducesResponseType(typeof(Result<ReceptionistOverviewDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetReceptionistOverview()
        {
            var query = new GetReceptionistOverviewQuery();

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }
        [Authorize(Roles = "Receptionist")]
        [HttpGet("dashboard/recent-activity")]
        [SwaggerOperation(
    Summary = "Get receptionist recent activity",
    Description = "Returns a list of recent activities performed by the receptionist such as adding to queue, rescheduling appointments, etc."
)]
        [ProducesResponseType(typeof(Result<List<ReceptionistActivityDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetReceptionistRecentActivity()
        {
            var result = await mediator.Send(new GetReceptionistRecentActivityQuery());

            return result.ToActionResult();
        }

        [Authorize(Roles = "Receptionist")]
        [HttpGet("dashboard/appointments")]
        [SwaggerOperation(
    Summary = "Get receptionist appointments dashboard",
    Description = "Returns a filtered list of appointments for the receptionist dashboard including search by patient name, appointment status, and appointment type when the receptionist opens the appointments section."
)]
        [ProducesResponseType(typeof(Result<ReceptionistAppointmentsDashboardDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetReceptionistAppointmentsDashboard(
    [FromQuery] GetReceptionistAppointmentsDashboardQuery query)
        {
            var result = await mediator.Send(query);

            return result.ToActionResult();
        }
        [Authorize(Roles = "Receptionist")]
        [HttpGet("dashboard/appointments/{id}")]
        [SwaggerOperation(
    Summary = "Get appointment details",
    Description = "Returns detailed information about a specific appointment including patient, doctor, schedule, status, payment status, and booking mode for receptionist review."
)]
        [ProducesResponseType(typeof(Result<AppointmentDetailsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAppointmentDetails([FromRoute] int id)
        {
            var result = await mediator.Send(new GetAppointmentDetailsQuery(id));

            return result.ToActionResult();
        }
        [HttpGet("doctor-patients")]
        [SwaggerOperation(
     Summary = "Get doctor patients (Receptionist view)",
     Description = "Returns paginated list of patients assigned to the receptionist's doctor with filters."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetPatients([FromQuery] GetPatientsQuery query)
        {
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }
        [HttpGet("{id}/doctor-info")]
        [SwaggerOperation(
          Summary = "Get patient details for doctor (Receptionist view)",
          Description = "Returns detailed patient info if the patient belongs to the receptionist's doctor."
      )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetPatientDoctorInfo(int id)
        {
            var result = await mediator.Send(new GetPatientDoctorInfoQuery(id));
            return result.ToActionResult();
        }
        [HttpGet("{patientId}/medical-history")]
        [SwaggerOperation(
        Summary = "Get patient medical history (Receptionist view)",
        Description = "Returns all medical records of a patient if they belong to the receptionist's doctor."
    )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetMedicalHistory(int patientId)
        {
            var result = await mediator.Send(
                new GetPatientMedicalHistoryQuery { PatientId = patientId});
               

            return result.ToActionResult();
        }
        [HttpGet("recent-patients")]
        [SwaggerOperation(
           Summary = "Get recent patients",
           Description = "Returns the most recently visited patients for the logged-in receptionist's doctor."
       )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetRecentPatients([FromQuery] GetRecentPatientsQuery query)
        {
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }
    }
}
