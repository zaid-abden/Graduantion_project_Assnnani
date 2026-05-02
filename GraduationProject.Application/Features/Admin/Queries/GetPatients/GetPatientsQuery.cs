using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Queries.GetPatients
{
	public record GetPatientsQuery : IRequest<Result<List<PatientListDto>>>;
}
