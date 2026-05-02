using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Queries.GetReceptionists
{
	public record GetReceptionistsQuery : IRequest<Result<List<ReceptionistListDto>>>;
}
