using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Features.Specializations.Commands.CreateSpecialization;
using GraduationProject.Application.Features.Specializations.Commands.DeleteSpecialization;
using GraduationProject.Application.Features.Specializations.Commands.UpdateSpecialization;
using GraduationProject.Application.Features.Specializations.Queries.GetAllSpeicializations;
using GraduationProject.Application.Features.Specializations.Queries.GetDoctorsBySpecialization;
using GraduationProject.Application.Features.Specializations.Queries.GetSpecializationById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationsController : ControllerBase
    {
        private readonly IMediator mediator;
        public SpecializationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet("GetAllSpecializations")]
       
        public async Task<IActionResult> GetAllSpecializations()
        {
            var result = await mediator.Send(new GetAllSpecilaizationQuery());
            return Ok(result);
        }

        [HttpGet("doctors-by-specialization")]

        public async Task<IActionResult> GetDoctorsBySpecialization([FromQuery] int id)
        {
            var result = await mediator.Send(new GetDoctorsBySpecializationQuery(id));
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecializationById(int id)
        {
            var result = await mediator.Send(new GetSpecializationByIdQuery(id));
            return result.ToActionResult();
        }
        [HttpPost("AddSpecialization")]
        public async Task<IActionResult> CreateSpecilaization([FromBody] CreateSpecializationCommand createSpecializationCommand)
        {
            var result = await mediator.Send(createSpecializationCommand);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSpecialization(int id, [FromBody] UpdateSpecializationCommand updateSpecializationCommand)
        {
            if (id != updateSpecializationCommand.Id)
            {
                return BadRequest("ID mismatch");
            }
            var result = await mediator.Send(updateSpecializationCommand);
            return result.ToActionResult();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpecialization(int id, DeleteSpecializationCommand deleteSpecializationCommand)
        {
            if (id != deleteSpecializationCommand.Id)
            {
                return BadRequest("ID mismatch");
            }

            var result = await mediator.Send(new DeleteSpecializationCommand(id));
            return result.ToActionResult();
        }
    }
}
