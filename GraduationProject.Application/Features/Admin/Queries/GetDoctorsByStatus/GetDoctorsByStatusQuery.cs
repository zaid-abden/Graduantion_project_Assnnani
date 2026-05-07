using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Admin.DTOs;
using GraduationProject.Data.Enums;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Queries.GetDoctorsByStatus
{
	public record GetDoctorsByStatusQuery(DoctorVerificationStatus? Status)
	: IRequest<Result<List<DoctorStatusDto>>>;
}
