using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Queries.GetRejectedUsers
{
	public record GetRejectedUsersQuery : IRequest<Result<List<RejectedUserDto>>>;
}
