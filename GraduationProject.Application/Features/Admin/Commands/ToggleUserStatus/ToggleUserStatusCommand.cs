using GraduationProject.Application.Common.Results;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Commands.ToggleUserStatus
{
	public record ToggleUserStatusCommand(string UserId) : IRequest<Result<bool>>;
}
