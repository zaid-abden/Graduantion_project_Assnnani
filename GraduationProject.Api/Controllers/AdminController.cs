using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Features.Admin.Commands.RejectUser;
using GraduationProject.Application.Features.Admin.Commands.SendEmail;
using GraduationProject.Application.Features.Admin.Commands.ToggleUserStatus;
using GraduationProject.Application.Features.Admin.DTOs;
using GraduationProject.Application.Features.Admin.Queries.GetAllUsers;
using GraduationProject.Application.Features.Admin.Queries.GetDoctorsOnly;
using GraduationProject.Application.Features.Admin.Queries.GetPatients;
using GraduationProject.Application.Features.Admin.Queries.GetPendingUsers;
using GraduationProject.Application.Features.Admin.Queries.GetReceptionists;
using GraduationProject.Application.Features.Admin.Queries.GetRejectedUsers;
using GraduationProject.Application.Features.Admin.Queries.GetStats;
using GraduationProject.Application.Features.Admin.Queries.GetStudents;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")] // حماية كاملة: لن يدخل هنا إلا من يحمل Role == Admin في التوكن الخاص به
	public class AdminController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly ICurrentUserService _currentUserService;

		public AdminController(IMediator mediator, ICurrentUserService currentUserService)
		{
			_mediator = mediator;
			_currentUserService = currentUserService;
		}

		#region Dashboard & Statistics

		[HttpGet("stats/summary")]
		public async Task<IActionResult> GetStats()
		{
			// السيستم يعرف أنك الأدمن من خلال التوكن تلقائياً
			var result = await _mediator.Send(new GetDashboardStatsQuery());
			return result.ToActionResult();
		}

		#endregion

		#region User Verification (Doctors & Students)

		[HttpGet("doctors/pending")]
		public async Task<IActionResult> GetPendingUsers()
		{
			var result = await _mediator.Send(new GetPendingUsersQuery());
			return result.ToActionResult();
		}

		[HttpPost("doctors/{id}/verify")]
		public async Task<IActionResult> Verify(string id, [FromBody] VerifyUserRequest request)
		{
			// id: هو معرف الطبيب المستهدف من الـ URL
			var result = await _mediator.Send(new VerifyUserCommand(id, request.Note));
			return result.ToActionResult();
		}

		[HttpPost("doctors/{id}/reject")]
		public async Task<IActionResult> Reject(string id, [FromBody] RejectUserRequest request)
		{
			var result = await _mediator.Send(new RejectUserCommand(id, request.Reason));
			return result.ToActionResult();
		}

		[HttpGet("doctors/rejected")]
		public async Task<IActionResult> GetRejected()
		{
			var result = await _mediator.Send(new GetRejectedUsersQuery());
			return result.ToActionResult();
		}

		#endregion

		#region User Management (All Roles)

		[HttpGet("users")]
		public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersQuery query)
		{
			var result = await _mediator.Send(query);
			return result.ToActionResult();
		}

		[HttpGet("users/doctors")]
		public async Task<IActionResult> GetDoctors()
		{
			var result = await _mediator.Send(new GetDoctorsQuery());
			return result.ToActionResult();
		}

		[HttpGet("users/patients")]
		public async Task<IActionResult> GetPatients()
		{
			var result = await _mediator.Send(new GetPatientsQuery());
			return result.ToActionResult();
		}

		[HttpGet("users/students")]
		public async Task<IActionResult> GetStudents()
		{
			var result = await _mediator.Send(new GetStudentsQuery());
			return result.ToActionResult();
		}

		[HttpGet("users/receptionists")]
		public async Task<IActionResult> GetReceptionists()
		{
			var result = await _mediator.Send(new GetReceptionistsQuery());
			return result.ToActionResult();
		}

		[HttpPut("users/{id}/toggle-status")]
		public async Task<IActionResult> ToggleStatus(string id)
		{
			var result = await _mediator.Send(new ToggleUserStatusCommand(id));
			return result.ToActionResult();
		}

		#endregion

		#region Communications

		[HttpPost("broadcast-email")]
		public async Task<IActionResult> SendBroadcastEmail([FromBody] SendEmailCommand command)
		{
			// الأدمن يحدد الوجهة (Role, User, or List) في الـ Body الخاص بالطلب
			var result = await _mediator.Send(command);
			return result.ToActionResult();
		}

		#endregion
	}
}
