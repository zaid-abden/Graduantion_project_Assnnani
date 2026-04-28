using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Scans.Commands.AddReviewScan;
using GraduationProject.Application.Features.Scans.Commands.CreateScan;
using GraduationProject.Application.Features.Scans.Commands.RejectScan;
using GraduationProject.Application.Features.Scans.Commands.ReopenScan;
using GraduationProject.Application.Features.Scans.Commands.StartReview;
using GraduationProject.Application.Features.Scans.Dtos;
using GraduationProject.Application.Features.Scans.Queries.GetScanDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScansController : ControllerBase
    {
        private readonly IMediator mediator;

        public ScansController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        [Authorize(Roles = "Doctor,Receptionist")]
        [SwaggerOperation(
          Summary = "Upload scan",
          Description = "Uploads a medical scan file and links it to a patient."
      )]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromForm] CreateScanCommand command)
        {
            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Doctor,Receptionist")]
        [SwaggerOperation(
          Summary = "Get scan details",
          Description = "Returns detailed information about a specific scan."
      )]
        [ProducesResponseType(typeof(Result<ScanDetailsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetScanDetailsQuery { Id = id };

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }

        [HttpPost("{scanId}/review")]
        [Authorize(Roles = "Doctor")]
        [SwaggerOperation(
          Summary = "Add scan review",
          Description = "Allows doctor to add findings and recommendations for a scan."
      )]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddReview(int scanId, [FromBody] AddReviewScanCommand command)
        {
            command.ScanId = scanId;

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPatch("{scanId}/start-review")]
        [Authorize(Roles = "Doctor")]
        [SwaggerOperation(
          Summary = "Start scan review",
          Description = "Marks scan as in-review so doctor can begin analyzing it."
      )]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> StartReview(int scanId)
        {
            var command = new StartReviewCommand(scanId);

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPatch("{scanId}/reject")]
        [Authorize(Roles = "Doctor")]
        [SwaggerOperation(
          Summary = "Reject scan",
          Description = "Marks scan as rejected if it's not valid for medical review."
      )]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Reject(int scanId)
        {
            var command = new RejectScanCommand(scanId);

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpPatch("{scanId}/reopen")]
        [Authorize(Roles = "Doctor")]
        [SwaggerOperation(
         Summary = "Reopen scan",
         Description = "Reopens a previously rejected or reviewed scan for further processing."
     )]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Reopen(int scanId)
        {
            var command = new ReopenScanCommand(scanId);

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
    }
}
