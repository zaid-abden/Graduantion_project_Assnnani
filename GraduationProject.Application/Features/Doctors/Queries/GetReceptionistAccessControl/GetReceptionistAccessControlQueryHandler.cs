using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Doctors.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetReceptionistAccessControl
{
    public class GetReceptionistAccessControlQueryHandler : IRequestHandler<GetReceptionistAccessControlQuery, Result<ReceptionistAccessControlDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetReceptionistAccessControlQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<ReceptionistAccessControlDto>> Handle(GetReceptionistAccessControlQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<ReceptionistAccessControlDto>.Failure(ResultStatus.Unauthorized, "Unauthorized access");

            var userId = currentUserService.UserId;

            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(d => d.UserId == userId, cancellationToken);

            if (doctor == null)
                return Result<ReceptionistAccessControlDto>.Failure(ResultStatus.NotFound, "Doctor not found");
            var receptionists = await unitOfWork.Receptionists.Query()
      .Where(r => r.DoctorId == doctor.DoctorId)
      .Select(r => new ReceptionistListItemDto
      {
          Id = r.ReceptionistId.ToString(),
          FullName = r.User.FullName,
          Email = r.User.Email!,
          PhoneNumber = r.User.PhoneNumber,
          IsActive = r.IsActive,
          CreatedAt = DateOnly.FromDateTime(r.User.CreatedAt),
          LastLoginDate = GetLastActive(r.User.LastLoginDateUtc)
      })
      .ToListAsync(cancellationToken);

            var dto = new ReceptionistAccessControlDto
            {
                Statistics = new ReceptionistStatisticsDto
                {
                    TotalReceptionists = receptionists.Count,
                    ActiveReceptionists = receptionists.Count(x => x.IsActive),
                    InactiveReceptionists = receptionists.Count(x => !x.IsActive)
                },

                Receptionists = receptionists
            };

            return Result<ReceptionistAccessControlDto>.Success(dto);
        }
        private static string GetLastActive(DateTimeOffset? lastLogin)
        {
            if (lastLogin is null)
                return "Never logged in";

            var diff = DateTimeOffset.UtcNow - lastLogin.Value;

            if (diff.TotalMinutes < 1)
                return "Active now";

            if (diff.TotalMinutes < 60)
                return $"{(int)diff.TotalMinutes} minutes ago";

            if (diff.TotalHours < 24)
                return $"{(int)diff.TotalHours} hours ago";

            if (diff.TotalDays < 7)
                return $"{(int)diff.TotalDays} days ago";

            if (diff.TotalDays < 30)
                return $"{(int)(diff.TotalDays / 7)} weeks ago";

            if (diff.TotalDays < 365)
                return $"{(int)(diff.TotalDays / 30)} months ago";

            return $"{(int)(diff.TotalDays / 365)} years ago";
        }
    }
}
