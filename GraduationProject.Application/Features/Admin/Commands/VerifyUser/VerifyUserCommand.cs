using GraduationProject.Application.Common.Results;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Commands.RejectUser
{
	public record VerifyUserCommand(string Id, string? Note) : IRequest<Result<bool>>;
}
