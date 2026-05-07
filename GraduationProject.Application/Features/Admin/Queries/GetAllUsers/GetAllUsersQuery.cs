using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Queries.GetAllUsers
{
	public record GetAllUsersQuery(
	string? SearchTerm = null,
	string? Role = null,
	//string? Status = null, // Pending, Active, Rejected
	string? Gender = null,
	int PageNumber = 1,
	int PageSize = 10
) : IRequest<Result<PagedUsersDto>>;
}
