using GraduationProject.Application.Common.Results;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Commands.RejectUser
{
	public record RejectUserCommand(string Id, string Reason) : IRequest<Result<bool>>;
}
