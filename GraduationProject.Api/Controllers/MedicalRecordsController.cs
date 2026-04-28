using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Features.MedicalRecords.Commands.AddMedicalRecord;
using GraduationProject.Application.Features.MedicalRecords.Commands.AddMedicalRecordAttachment;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly IMediator mediator;

        public MedicalRecordsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        [SwaggerOperation(
            Summary = "Add medical record",
            Description = "Creates a new medical record (visit) for a patient."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddMedicalRecord([FromBody] AddMedicalRecordCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpPost("{medicalRecordId}/attachments")]
        [SwaggerOperation(
         Summary = "Upload attachment to medical record",
         Description = "Uploads images, X-rays or documents to a medical record."
     )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UploadAttachment(
         int medicalRecordId,
         IFormFile file)
        {
            var command = new AddMedicalRecordAttachmentCommand
            {
                MedicalRecordId = medicalRecordId,
                File = file
            };

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
