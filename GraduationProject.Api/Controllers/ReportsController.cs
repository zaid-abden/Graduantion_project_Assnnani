using GraduationProject.Application.Features.Financial.Queries.get_Financial_report;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/v1/doctor/reports")]
[Authorize(Roles = "Doctor")] // تأكد أن الطبيب فقط من يصل إليها
public class ReportsController : ControllerBase
{
	private readonly IMediator _mediator;
	public ReportsController(IMediator mediator) => _mediator = mediator;

	[HttpGet("finance")]
	public async Task<IActionResult> GetFinanceReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
	{
		// استخراج الـ ID الخاص بالطبيب من الـ Token
		var doctorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

		var query = new GetFinancialReportQuery(doctorId, startDate, endDate);
		var result = await _mediator.Send(query);

		return Ok(result);
	}
}