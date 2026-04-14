using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Features.DoctorSchedule.Commands.CreateSchedule;
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

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorSchedulesController : ControllerBase
    {
        private readonly IMediator mediator;
        public DoctorSchedulesController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet("Get-All-Schedules")]
        public async Task<IActionResult> GetAllSchedules()
        {
            var query = new GetAllSchedulesQuery();
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("Get-All-Active-Shedules")]
        public async Task<IActionResult> GetActiveSchedulesForDoctor()
        {
            var query = new GetAllActiveScheduleQuery();
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("Get-All-Not-Active-Shedules")]
        public async Task<IActionResult> GetNotActiveSchedulesForDoctor()
        {
            var query = new GetAllNotActiveScheduleQuery();
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }




        [HttpGet("Get-Schedule-By-Id/{id}")]    
        public async Task<IActionResult> GetScheduleById(int id)
        {
            var query = new GetScheduleByIdQuery(id);
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("Get-Schedule-Details-By-Doctor/{id}")]

        public async Task<IActionResult> GetScheduleDetailsByDoctor(int id)
        {
            var query = new GetAllActiveScheduleForDoctorQuery(id);
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }
        [HttpGet("by-doctor-day")]
        public async Task<IActionResult> GetSchedulesByDoctorAndDay([FromQuery] int doctorId, [FromQuery] WeekDay day)
        {
            var query = new GetSchedulesByDoctorDayQuery(doctorId, (WeekDay)day);
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }
        [HttpPost("Create-New-Schedule")]
        public async Task<IActionResult> CreateSchedule([FromBody] CreateScheduleCommand createScheduleCommand)
        {
            var result=await mediator.Send(createScheduleCommand);
            return result.ToActionResult();
        }
        [HttpPut("Update-Schedule/{id}")]
        public async Task<IActionResult> CreateSchedule([FromBody] UpdateScheduleCommand updateScheduleCommand,int id)
        {
            if(id!=updateScheduleCommand.ScheduleId)
            {
                return BadRequest("Schedule ID mismatch between URL and body.");
            }
            var result = await mediator.Send(updateScheduleCommand);
            return result.ToActionResult();
        }
        [HttpDelete("Delete-Schedule/{id}")]
        public async Task<IActionResult> DeleteSchedule([FromBody] DeleteScheduleCommand deleteScheduleCommand, int id)
        {
            if (id != deleteScheduleCommand.ScheduleId)
            {
                return BadRequest("Schedule ID mismatch between URL and body.");
            }
            var result = await mediator.Send(deleteScheduleCommand);
            return result.ToActionResult();
        }

        [HttpPut("Make-Schedule-Active/{id}")]
        public async Task<IActionResult> MakeScheduleActive(MakeAScheduleActiveCommand makeAScheduleActiveCommand,int id)
        {
            if(id!= makeAScheduleActiveCommand.ScheduleId)
            {
                return BadRequest("Schedule ID mismatch between URL and body.");
            }
            var result = await mediator.Send(new MakeAScheduleActiveCommand(makeAScheduleActiveCommand.ScheduleId));
            return result.ToActionResult();
        }
    }
}
