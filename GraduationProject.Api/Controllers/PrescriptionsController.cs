using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Features.Prescriptions.Commands.AddPrescription;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IMediator mediator;

        public PrescriptionsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        [SwaggerOperation(
         Summary = "Add prescription",
         Description = "Creates a prescription for a patient with multiple items (medications)."
     )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddPrescription([FromBody] AddPrescriptionCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
