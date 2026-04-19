using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Features.Doctors.Commands.UpdateDoctorProfile;
using GraduationProject.Application.Features.Doctors.Queries.GetDoctorProfile;
using GraduationProject.Application.Features.Doctors.Queries.GetDoctorStatistics;
using GraduationProject.Application.Features.Doctors.Queries.GetDoctorTodaySummary;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	//[Authorize]
	public class DoctorsController : ControllerBase
	{
		private readonly IMediator mediator;
		public DoctorsController(IMediator mediator)
		{
			this.mediator = mediator;
		}

		[HttpGet("statistics/{doctorId}")]
		public async Task<IActionResult> GetStatistics([FromRoute] int doctorId)
		{
			var query = new GetDoctorStatisticsQuery(doctorId);
			var result = await mediator.Send(query);

			return result.ToActionResult();
		}

		[HttpGet("today-summary/{doctorId}")]
		public async Task<IActionResult> GetTodaySummary([FromRoute] int doctorId)
		{
			var result = await mediator.Send(new GetDoctorTodaySummaryQuery(doctorId));

			return result.ToActionResult();
		}

		[HttpGet("profile/{doctorId}")]
		public async Task<IActionResult> GetProfile([FromRoute] int doctorId)
		{
			var result = await mediator.Send(new GetDoctorProfileQuery(doctorId));

			return result.ToActionResult();
		}

		[HttpPost("updateprofile/{doctorId}")]
		public async Task<IActionResult> UpdateProfile([FromRoute] int doctorId, [FromBody] UpdateDoctorProfileCommand command)
		{
			command.DoctorId = doctorId;

			var result = await mediator.Send(command);

			return result.ToActionResult();
		}
	}
}
