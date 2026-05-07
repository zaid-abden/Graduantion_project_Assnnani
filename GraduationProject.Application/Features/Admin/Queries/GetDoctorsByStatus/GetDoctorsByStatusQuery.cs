using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Admin.DTOs;
using GraduationProject.Data.Enums;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Queries.GetDoctorsByStatus;

// الـ Body اللي هيتبعت من الفرونت
public record GetDoctorsByStatusQuery(DoctorVerificationStatus? Status)
	: IRequest<Result<List<DoctorStatusDto>>>;