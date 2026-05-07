using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Queries.GetPendingUsers;

public record GetPendingUsersQuery : IRequest<Result<List<PendingDoctorsDetailsDto>>>;