using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Queries.GetDoctorsOnly
{
	public record GetDoctorsQuery : IRequest<Result<List<DoctorListDto>>>;
}
