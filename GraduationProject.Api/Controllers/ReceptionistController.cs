using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Features.Receptionist.Queries.GetAllPatients;
using GraduationProject.Application.Features.Receptionist.Queries.PatientInfo;
using GraduationProject.Application.Features.Receptionist.Queries.ReceptionistDashboard;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceptionistController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReceptionistController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("ReceptiostDashBord/{id}")]
        public async Task<IActionResult> ReceptiostDashBord([FromRoute] int id)
        {
            var result = await _mediator.Send(new GetReceptionistDashboardQuery(id));
            return result.ToActionResult();
        }
        [HttpGet("PatientInfo/{id}")]
        public async Task<IActionResult> PatientInfo([FromRoute] int id)
        {
            var result = await _mediator.Send(new GetPatientInfoQuery(id));
            return result.ToActionResult();
        }
        [HttpGet("GetAllpatientForReceptionist")]
        public async Task<IActionResult> GetAllpatientForReceptionist([FromQuery] int receptionistId,
                                                                      [FromQuery] int? doctorId,
                                                                      [FromQuery] string? status,
                                                                      [FromQuery] string? search)
        {
            var result = await _mediator.Send(new GetPatientsListQuery
            {
                ReceptionistId = receptionistId,
                DoctorId = doctorId,
                Status = status,
                Search = search
            });
            return result.ToActionResult();
        }
    }
}
