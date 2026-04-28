using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Common.PaginatedResults;
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Appointments.Commands.AddAppointment;
using GraduationProject.Application.Features.Appointments.Commands.CancelAppointment;
using GraduationProject.Application.Features.Appointments.Commands.ConfirmAppointment;
using GraduationProject.Application.Features.Appointments.Commands.MarkAsArrived;
using GraduationProject.Application.Features.Appointments.Commands.RescheduleAppointment;
using GraduationProject.Application.Features.Appointments.Commands.UpdateAppointment;
using GraduationProject.Application.Features.Appointments.Dtos;
using GraduationProject.Application.Features.Appointments.Queries.GetAllApointmentsWithPagination;
using GraduationProject.Application.Features.Appointments.Queries.GetAllAppointments;
using GraduationProject.Application.Features.Appointments.Queries.GetAppointmentById;
using GraduationProject.Application.Features.Appointments.Queries.GetAvailableDoctorsWithSlots;
using GraduationProject.Application.Features.Appointments.Queries.GetAvailableSlots;
using GraduationProject.Application.Features.Patients.commands.updatepationtcommand;
using GraduationProject.Data.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IMediator mediator;

        public AppointmentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
       // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Get all appointments (Admin)",
    Description = "Returns all appointments in the system with patient and doctor details."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            var result = await mediator.Send(new GetAllAppointmentsQuery());
            return result.ToActionResult();
        }
        [HttpGet("{id}")]
       // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Get appointment by ID (Admin)",
    Description = "Returns appointment details for the given ID."
)]
        [ProducesResponseType(typeof(Result<AppointmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetAppointmentByIdQuery(id);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }

        [HttpGet("paginated")]
        //[Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Get all appointments with pagination (Admin)",
    Description = "Returns paginated list of appointments with patient and doctor details."
)]
        [ProducesResponseType(typeof(Result<PaginatedResult<AppointmentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllPaginated(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
        {
            var query = new GetAllApointmentsWithPaginationQuery(pageNumber, pageSize);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }


        [HttpPost()]
        [SwaggerOperation(
          Summary = "Book an appointment",
          Description = "Allows a patient to book an available schedule slot."
      )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> BookAppointment([FromBody] AddAppointmentCommand command, [FromQuery] AppointmentType appointmentType)
        {
            command.appointmentType = appointmentType;
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
    Summary = "Update an appointment",
    Description = "Allows updating appointment details such as schedule slot, notes, and payment method."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAppointmentCommand command)
        {

            if (id != command.AppointmentId)
                return BadRequest("Ids mis match");
            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpGet("available-doctors")]
        [SwaggerOperation(
    Summary = "Get available doctors",
    Description = "Returns doctors who have available slots filtered by date, location, and specialization."
)]
        [ProducesResponseType(typeof(Result<List<AvailableDoctorWithSlotsDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAvailableDoctors(
    [FromQuery] DateOnly date,
    [FromQuery] string? location,
    [FromQuery] int? specialization)
        {
            if (date < DateOnly.FromDateTime(DateTime.Today))
            {
                return BadRequest(Result<string>.Failure(
                    ResultStatus.Failure,
                    "Date cannot be in the past."
                ));
            }

            var query = new GetAvailableDoctorsWithSlotsQuery(
                date,
                location,
                specialization);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }

        [HttpGet("available-slots")]
        [SwaggerOperation(
    Summary = "Get available slots",
    Description = "Returns available slots for a specific doctor on a given date."
)]
        [ProducesResponseType(typeof(Result<List<AvailableSlotDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAvailableSlots(
    [FromQuery] int doctorId,
    [FromQuery] DateOnly date)
        {
            var query = new GetAvailableSlotsQuery(doctorId, date);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }

        [HttpPatch("{id}/cancel")]
        [Authorize(Roles = "Patient")]
        [SwaggerOperation(
    Summary = "Cancel my appointment (Patient)",
    Description = "Allows the logged-in patient to cancel their own appointment only."
)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CancelAppointment(int id)
        {
           
          

            var command = new CancelAppointmentCommand(id);

            var result = await mediator.Send(command);

            return result.ToActionResult();
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

        [HttpPatch("{id}/reschedule")]
        [Authorize(Roles = "Patient")]
        [SwaggerOperation(
    Summary = "Reschedule appointment",
    Description = "Allows patient to change appointment slot if new slot is available."
)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Reschedule(int id, [FromQuery] int newSlotId)
        {
            

            var command = new RescheduleAppointmentCommand(id, newSlotId);

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPatch("{id}/arrive")]
        //[Authorize(Roles = "Receptionist,Doctor")] 
        [SwaggerOperation(
    Summary = "Mark appointment as arrived",
    Description = "Marks the appointment as arrived when the patient checks in."
)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> MarkAsArrived(int id)
        {
            var command = new MarkAsArrivedCommand(id);

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
    }
}
