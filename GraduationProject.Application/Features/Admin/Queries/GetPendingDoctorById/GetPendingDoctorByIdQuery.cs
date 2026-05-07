using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Queries.GetPendingDoctorById
{
	public record GetPendingDoctorByIdQuery(int DoctorId) : IRequest<Result<PendingDoctorDetailsDto>>;
}
