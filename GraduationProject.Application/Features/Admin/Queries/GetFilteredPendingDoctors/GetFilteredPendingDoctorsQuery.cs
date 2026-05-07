using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Queries.GetFilteredPendingDoctors
{
	public record FilterPendingDoctorsQuery(
	string? SearchTerm = null,
	int PageNumber = 1,
	int PageSize = 10
) : IRequest<Result<FilterPendingDoctorsResultDto>>;
}
