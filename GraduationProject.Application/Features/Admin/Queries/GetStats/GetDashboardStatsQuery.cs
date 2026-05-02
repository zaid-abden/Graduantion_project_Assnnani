using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Queries.GetStats;

public record GetDashboardStatsQuery : IRequest<Result<DashboardStatsDto>>;