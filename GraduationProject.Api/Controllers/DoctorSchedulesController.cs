using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Features.DoctorSchedule.Commands.CreateSchedule;
using GraduationProject.Application.Features.DoctorSchedule.Commands.DeactiveSchedule;
using GraduationProject.Application.Features.DoctorSchedule.Commands.DeleteSchedule;
using GraduationProject.Application.Features.DoctorSchedule.Commands.MakeAScheduleActive;
using GraduationProject.Application.Features.DoctorSchedule.Commands.UpdateSchedule;
using GraduationProject.Application.Features.DoctorSchedule.Queries.GellAllActiveSchedule;
using GraduationProject.Application.Features.DoctorSchedule.Queries.GetAllActiveScheduleForDoctor;
using GraduationProject.Application.Features.DoctorSchedule.Queries.GetAllNotActiveSchedule;
using GraduationProject.Application.Features.DoctorSchedule.Queries.GetAllSchedules;
using GraduationProject.Application.Features.DoctorSchedule.Queries.GetScheduleById;
using GraduationProject.Application.Features.DoctorSchedule.Queries.GetSchedulesByDoctor_Day;
using GraduationProject.Data.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GraduationProject.Api.Controllers
{
    //    [Route("api/[controller]")]
    //    [ApiController]
    //    public class DoctorSchedulesController : ControllerBase
    //    {
    //        private readonly IMediator mediator;
    //        public DoctorSchedulesController(IMediator mediator)
    //        {
    //            this.mediator = mediator;
    //        }
    //        [HttpGet("Get-All-Schedules")]
    //        public async Task<IActionResult> GetAllSchedules()
    //        {
    //            var query = new GetAllSchedulesQuery();
    //            var result = await mediator.Send(query);
    //            return result.ToActionResult();
    //        }

    //        [HttpGet("Get-All-Active-Shedules")]
    //        public async Task<IActionResult> GetActiveSchedulesForDoctor()
    //        {
    //            var query = new GetAllActiveScheduleQuery();
    //            var result = await mediator.Send(query);
    //            return result.ToActionResult();
    //        }

    //        [HttpGet("Get-All-Not-Active-Shedules")]
    //        public async Task<IActionResult> GetNotActiveSchedulesForDoctor()
    //        {
    //            var query = new GetAllNotActiveScheduleQuery();
    //            var result = await mediator.Send(query);
    //            return result.ToActionResult();
    //        }




    //        [HttpGet("Get-Schedule-By-Id/{id}")]    
    //        public async Task<IActionResult> GetScheduleById(int id)
    //        {
    //            var query = new GetScheduleByIdQuery(id);
    //            var result = await mediator.Send(query);
    //            return result.ToActionResult();
    //        }

    //        [HttpGet("Get-Schedule-Details-By-Doctor/{id}")]

    //        public async Task<IActionResult> GetScheduleDetailsByDoctor(int id)
    //        {
    //            var query = new GetAllActiveScheduleForDoctorQuery(id);
    //            var result = await mediator.Send(query);
    //            return result.ToActionResult();
    //        }
    //        [HttpGet("by-doctor-day")]
    //        public async Task<IActionResult> GetSchedulesByDoctorAndDay([FromQuery] int doctorId, [FromQuery] WeekDay day)
    //        {
    //            var query = new GetSchedulesByDoctorDayQuery(doctorId, (WeekDay)day);
    //            var result = await mediator.Send(query);
    //            return result.ToActionResult();
    //        }
    //        [HttpPost("Create-New-Schedule")]
    //        public async Task<IActionResult> CreateSchedule([FromBody] CreateScheduleCommand createScheduleCommand)
    //        {
    //            var result=await mediator.Send(createScheduleCommand);
    //            return result.ToActionResult();
    //        }
    //        [HttpPut("Update-Schedule/{id}")]
    //        public async Task<IActionResult> CreateSchedule([FromBody] UpdateScheduleCommand updateScheduleCommand,int id)
    //        {
    //            if(id!=updateScheduleCommand.ScheduleId)
    //            {
    //                return BadRequest("Schedule ID mismatch between URL and body.");
    //            }
    //            var result = await mediator.Send(updateScheduleCommand);
    //            return result.ToActionResult();
    //        }
    //        [HttpDelete("Delete-Schedule/{id}")]
    //        public async Task<IActionResult> DeleteSchedule([FromBody] DeleteScheduleCommand deleteScheduleCommand, int id)
    //        {
    //            if (id != deleteScheduleCommand.ScheduleId)
    //            {
    //                return BadRequest("Schedule ID mismatch between URL and body.");
    //            }
    //            var result = await mediator.Send(deleteScheduleCommand);
    //            return result.ToActionResult();
    //        }

    //        [HttpPatch("Make-Schedule-Active/{id}")]
    //        public async Task<IActionResult> MakeScheduleActive(MakeAScheduleActiveCommand makeAScheduleActiveCommand,int id)
    //        {
    //            if(id!= makeAScheduleActiveCommand.ScheduleId)
    //            {
    //                return BadRequest("Schedule ID mismatch between URL and body.");
    //            }
    //            var result = await mediator.Send(new MakeAScheduleActiveCommand(makeAScheduleActiveCommand.ScheduleId));
    //            return result.ToActionResult();
    //        }
    //        [HttpPatch("{id}/deactivate")]
    //        [SwaggerOperation(
    //    Summary = "Deactivate a doctor schedule",
    //    Description = "Soft deletes (deactivates) a doctor schedule if it has no active appointments."
    //)]
    //        [ProducesResponseType(StatusCodes.Status200OK)]
    //        [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //        [ProducesResponseType(StatusCodes.Status404NotFound)]
    //        public async Task<IActionResult> Deactivate(int id)
    //        {
    //            var result = await mediator.Send(
    //                new DeactivateScheduleCommand (id));

    //            return result.ToActionResult();
    //        }
    //    }
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorSchedulesController : ControllerBase
    {
        private readonly IMediator mediator;

        public DoctorSchedulesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

       
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all doctor schedules",
            Description = "Retrieves all schedules (active and inactive) for all doctors."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllSchedules()
        {
            var result = await mediator.Send(new GetAllSchedulesQuery());
            return result.ToActionResult();
        }

      
        [HttpGet("active")]
        [SwaggerOperation(
            Summary = "Get active schedules",
            Description = "Retrieves all active schedules for doctors."
        )]
        public async Task<IActionResult> GetActiveSchedulesForDoctor()
        {
            var result = await mediator.Send(new GetAllActiveScheduleQuery());
            return result.ToActionResult();
        }

     
        [HttpGet("inactive")]
        [SwaggerOperation(
            Summary = "Get inactive schedules",
            Description = "Retrieves all deactivated/inactive schedules."
        )]
        public async Task<IActionResult> GetNotActiveSchedulesForDoctor()
        {
            var result = await mediator.Send(new GetAllNotActiveScheduleQuery());
            return result.ToActionResult();
        }

      
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get schedule by ID",
            Description = "Retrieves a specific schedule using its ID."
        )]
        public async Task<IActionResult> GetScheduleById(int id)
        {
            var result = await mediator.Send(new GetScheduleByIdQuery(id));
            return result.ToActionResult();
        }

        [HttpGet("doctor/{doctorId}/active")]
        [SwaggerOperation(
            Summary = "Get active schedules for doctor",
            Description = "Retrieves active schedules for a specific doctor."
        )]
        public async Task<IActionResult> GetScheduleDetailsByDoctor(int doctorId)
        {
            var result = await mediator.Send(new GetAllActiveScheduleForDoctorQuery(doctorId));
            return result.ToActionResult();
        }

        
        [HttpGet("doctor/{doctorId}/day/{day}")]
        [SwaggerOperation(
            Summary = "Get schedules by doctor and day",
            Description = "Retrieves schedules filtered by doctor and weekday."
        )]
        public async Task<IActionResult> GetSchedulesByDoctorAndDay(int doctorId, WeekDay day)
        {
            var result = await mediator.Send(new GetSchedulesByDoctorDayQuery(doctorId, day));
            return result.ToActionResult();
        }

     
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create new schedule",
            Description = "Creates a new doctor schedule."
        )]
        public async Task<IActionResult> CreateSchedule([FromBody] CreateScheduleCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

       
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update schedule",
            Description = "Updates an existing doctor schedule."
        )]
        public async Task<IActionResult> UpdateSchedule(int id, [FromBody] UpdateScheduleCommand command)
        {
            if (id != command.ScheduleId)
                return BadRequest("Schedule ID mismatch between URL and body.");

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete schedule",
            Description = "Permanently deletes a doctor schedule."
        )]
        public async Task<IActionResult> DeleteSchedule(int id, [FromBody] DeleteScheduleCommand command)
        {
            if (id != command.ScheduleId)
                return BadRequest("Schedule ID mismatch between URL and body.");

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

     
        [HttpPatch("{id}/activate")]
        [SwaggerOperation(
            Summary = "Activate schedule",
            Description = "Reactivates a previously deactivated schedule."
        )]
        public async Task<IActionResult> MakeScheduleActive(int id)
        {
            var result = await mediator.Send(new MakeAScheduleActiveCommand(id));
            return result.ToActionResult();
        }

       
        [HttpPatch("{id}/deactivate")]
        [SwaggerOperation(
            Summary = "Deactivate schedule",
            Description = "Soft deletes a schedule if it has no active appointments."
        )]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await mediator.Send(new DeactivateScheduleCommand(id));
            return result.ToActionResult();
        }
    }
}
