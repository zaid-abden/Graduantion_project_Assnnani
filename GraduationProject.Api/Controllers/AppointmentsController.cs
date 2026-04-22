using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Features.Appointments.Commands.AddAppointment;
using MediatR;
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
        [HttpPost()]
        [SwaggerOperation(
          Summary = "Book an appointment",
          Description = "Allows a patient to book an available schedule slot."
      )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> BookAppointment([FromBody] AddAppointmentCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
